using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;
using System.Data.Common;
using System.Globalization;
using System;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using Moses.Core.Data.EFExtensions;
namespace Moses.Data
{
    /// <summary>
    /// Supports shaping DbCommand and DbDataReader as CLR instances.
    /// </summary>
    /// <remarks>
    /// This type is thread-safe. For performance reasons, static instances of this type
    /// should be shared wherever possible. Note that a single instance of the Materializer
    /// cannot be used with two command or readers returning different fields or the same
    /// fields in a different order. To develop custom optimization behaviors, implement
    /// methods with the IMaterializerOptimizedMethodAttribute.
    /// </remarks>
    /// <typeparam name="T">CLR type to materialize.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Materializer")]
    public sealed class Materializer<T>
    {
        private static readonly ParameterExpression s_recordParameter = Expression.Parameter(typeof(IDataRecord), "record");
        private static readonly MethodInfo s_fieldOfTOrdinalMethod = typeof(DataExtensions).GetMethod("Field", new Type[] { typeof(IDataRecord), typeof(int) });
        private static readonly MethodInfo s_fieldOfTColumnNameMethod = typeof(DataExtensions).GetMethod("Field", new Type[] { typeof(IDataRecord), typeof(string) });

        private readonly Expression<Func<IDataRecord, T>> userSuppliedShaper;
        private readonly object syncLock = new object();

        private Func<IDataRecord, T> shaperDelegate;
        private ReadOnlyCollection<string> fieldNames;

        /// <summary>
        /// Default constructor. Instances of T are materialized by assigning field values to
        /// writable properties on T having the same name. By default, allows fields
        /// that do not have corresponding properties and properties that do not have corresponding
        /// fields.
        /// </summary>
        public Materializer()
        {
        }

        /// <summary>
        /// Creates a materializer for the given EDM type. Assumes that a column exists in the result
        /// set for every property of the EDM type.
        /// </summary>
        /// <remarks>
        /// Beyond requiring that all properties of the type are available, no type validation
        /// is performed.
        /// </remarks>
        /// <param name="structuralType">EDM type for which to create a Materializer.</param>
        public Materializer(StructuralType structuralType)
            : this(GetStructuralTypeShaper(structuralType))
        {
        }

        /// <summary>
        /// Instances of T are materialized using the given shaper. For every row in the result,
        /// the shaper is evaluated.
        /// </summary>
        /// <param name="shaper">Describes how reader rows are transformed into typed results.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public Materializer(Expression<Func<IDataRecord, T>> shaper)
        {
            this.userSuppliedShaper = shaper;
        }

        /// <summary>
        /// Materializes the results of the given command.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Shaped results.</returns>
        public IEnumerable<T> Materialize(DbCommand command)
        {
            return Materialize(command, CommandBehavior.Default);
        }

        /// <summary>
        /// Materializes the results of the given command.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="commandBehavior">Command behavior to use when executing the command.</param>
        /// <returns>Shaped results.</returns>
        public IEnumerable<T> Materialize(DbCommand command, CommandBehavior commandBehavior)
        {
            Utility.CheckArgumentNotNull(command, "command");

            using (command.Connection.CreateConnectionScope())
            {
                using (DbDataReader reader = command.ExecuteReader(commandBehavior))
                {
                    foreach (T element in this.Materialize(reader))
                    {
                        yield return element;
                    }
                }
            }
        }

        /// <summary>
        /// Materializes rows in the given reader.
        /// </summary>
        /// <param name="reader">Results to materialize.</param>
        /// <returns>Shaped results.</returns>
        public IEnumerable<T> Materialize(DbDataReader reader)
        {
            Utility.CheckArgumentNotNull(reader, "reader");

            bool first = true;
            while (reader.Read())
            {
                if (first)
                {
                    InitializeShaper(reader);
                    first = false;
                }

                yield return this.shaperDelegate(reader);
            }

            yield break;
        }

        private void InitializeShaper(IDataRecord record)
        {
            // Determine the layout of the record.
            if (null != this.fieldNames)
            {
                // If a record layout has already been established, make sure the current
                // record is compatible with it.
                ValidateFieldNames(record);
            }
            else
            {
                // Initialize a new shaper delegate within a lock (first one wins). 
                lock (this.syncLock)
                {
                    if (null != this.fieldNames)
                    {
                        // another thread beat us to it...
                        ValidateFieldNames(record);
                    }
                    else
                    {
                        // if the user didn't provide an explicit shaper, generate a default shaper
                        // based on the element type and the record layout.
                        ReadOnlyCollection<string> recordFieldNames = GetFieldNames(record);
                        Expression<Func<IDataRecord, T>> shaper = this.userSuppliedShaper ?? GetDefaultShaper(recordFieldNames);

                        // optimize the expression
                        Func<IDataRecord, T> compiledShaper = OptimizingExpressionVisitor
                            .Optimize(recordFieldNames, shaper)
                            .Compile();

                        // lock down the Materializer instance to use the (first encountered) field information and delegate
                        this.fieldNames = recordFieldNames;
                        this.shaperDelegate = compiledShaper;
                    }
                }
            }
        }

        private static ReadOnlyCollection<string> GetFieldNames(IDataRecord record)
        {
            List<string> fieldNames = new List<string>(record.FieldCount);
            fieldNames.AddRange(Enumerable.Range(0, record.FieldCount)
                .Select(i => record.GetName(i)));
            return fieldNames.AsReadOnly();
        }

        private void ValidateFieldNames(IDataRecord record)
        {
            if (this.fieldNames.Count != record.FieldCount ||
                this.fieldNames.Where((fieldName, ordinal) => record.GetName(ordinal) != fieldName)
                .Any())
            {
                throw new InvalidOperationException(Messages.IncompatibleReader);
            }
        }

        private static Expression<Func<IDataRecord, T>> GetDefaultShaper(ReadOnlyCollection<string> fieldNames)
        {
            ConstructorInfo defaultConstructor = GetDefaultConstructor();

            // figure out which fields/properties have corresponding columns
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            int ordinal = 0;
            foreach (string columnName in fieldNames)
            {
                MemberBinding memberBinding;
                if (TryCreateMemberBinding(columnName, ordinal, out memberBinding))
                {
                    memberBindings.Add(memberBinding);
                }
                ordinal++;
            }

            // record => new T { ColumnName = record.Field<TColumn>(columnOrdinal), ... }
            return Expression.Lambda<Func<IDataRecord, T>>(
                Expression.MemberInit(
                    Expression.New(defaultConstructor),
                    memberBindings),
                s_recordParameter);
        }

        private static Expression<Func<IDataRecord, T>> GetStructuralTypeShaper(StructuralType structuralType)
        {
            ConstructorInfo defaultConstructor = GetDefaultConstructor();

            // we expect to find a column for every 'structural' member of the type
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            foreach (EdmProperty property in structuralType.Members.OfType<EdmProperty>())
            {
                MemberBinding memberBinding;
                if (TryCreateMemberBinding(property.Name, null, out memberBinding))
                {
                    memberBindings.Add(memberBinding);
                }
            }

            // record => new T { ColumnName = record.Field<TColumn>("propertyName1"), ... }
            return Expression.Lambda<Func<IDataRecord, T>>(
                Expression.MemberInit(
                    Expression.New(defaultConstructor),
                    memberBindings),
                s_recordParameter);
        }

        private static ConstructorInfo GetDefaultConstructor()
        {
            ConstructorInfo defaultConstructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (null == defaultConstructor)
            {
                throw new InvalidOperationException(Messages.UnableToCreateDefaultMaterializeDelegate);
            }
            return defaultConstructor;
        }

        private static bool TryCreateMemberBinding(string columnName, int? ordinal, out MemberBinding memberBinding)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(columnName);
            if (null != propertyInfo)
            {
                if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanWrite)
                {
                    memberBinding = Expression.Bind(propertyInfo.GetSetMethod(), CreateGetValueCall(propertyInfo.PropertyType, columnName, ordinal));
                    return true;
                }
            }
            memberBinding = null;
            return false;
        }

        private static Expression CreateGetValueCall(Type type, string columnName, int? ordinal)
        {
            MethodInfo fieldOfTMethod;
            Expression fieldArgument;

            if (ordinal.HasValue)
            {
                fieldOfTMethod = s_fieldOfTOrdinalMethod.MakeGenericMethod(type);
                fieldArgument = Expression.Constant(ordinal.Value);
            }
            else
            {
                fieldOfTMethod = s_fieldOfTColumnNameMethod.MakeGenericMethod(type);
                fieldArgument = Expression.Constant(columnName);
            }

            return Expression.Call(fieldOfTMethod, s_recordParameter, fieldArgument);
        }

        /// <summary>
        /// LINQ expression visitor that optimizes method call expressions referencing methods with
        /// the IMaterializerMethodAttribute.
        /// </summary>
        private class OptimizingExpressionVisitor : ExpressionVisitor
        {
            private readonly ReadOnlyCollection<string> fieldNames;
            private readonly ParameterExpression recordParameter;

            private OptimizingExpressionVisitor(ReadOnlyCollection<string> fieldNames, ParameterExpression recordParameter)
            {
                this.fieldNames = fieldNames;
                this.recordParameter = recordParameter;
            }

            internal static Expression<Func<IDataRecord, T>> Optimize(ReadOnlyCollection<string> fieldNames, Expression<Func<IDataRecord, T>> shaper)
            {
                Utility.CheckArgumentNotNull(fieldNames, "fieldNames");
                Utility.CheckArgumentNotNull(shaper, "shaper");

                OptimizingExpressionVisitor visitor = new OptimizingExpressionVisitor(fieldNames, shaper.Parameters.Single());

                return (Expression<Func<IDataRecord, T>>)visitor.Visit(shaper);
            }

            protected override Expression VisitMethodCall(MethodCallExpression m)
            {
                Expression result = base.VisitMethodCall(m);

                if (result.NodeType == ExpressionType.Call)
                {
                    m = (MethodCallExpression)result;

                    MaterializerOptimizedMethodAttribute attribute = m.Method.GetCustomAttributes(typeof(MaterializerOptimizedMethodAttribute), false)
                        .Cast<MaterializerOptimizedMethodAttribute>()
                        .SingleOrDefault(); // multiple attributes not permitted; not inherited

                    if (null != attribute)
                    {
                        return attribute.Optimizer.OptimizeMethodCall(this.fieldNames, this.recordParameter, m);
                    }
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Attach this attribute to a method that can be locally rewritten to optimize Materializer
    /// performance.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Materializer")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class MaterializerOptimizedMethodAttribute : Attribute
    {
        private readonly IMaterializerMethodOptimizer optimizer;

        /// <summary>
        /// Construct attribute.
        /// </summary>
        /// <param name="optimizerType">A type implementing the IMaterializerMethodOptimizer interface
        /// that can be used to optimize MethodCallExpressions referencing the attributed method. The
        /// type must have a public default constructor.</param>
        public MaterializerOptimizedMethodAttribute(Type optimizerType)
        {
            Utility.CheckArgumentNotNull(optimizerType, "optimizerType");

            ConstructorInfo defaultConstructor = optimizerType.GetConstructor(Type.EmptyTypes);

            if (!typeof(IMaterializerMethodOptimizer).IsAssignableFrom(optimizerType) ||
                null == defaultConstructor)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Messages.InvalidOptimizerType, typeof(IMaterializerMethodOptimizer)), "optimizerType");
            }

            this.optimizer = (IMaterializerMethodOptimizer)defaultConstructor.Invoke(null);
        }

        /// <summary>
        /// Gets type of optimizer.
        /// </summary>
        public Type OptimizerType
        {
            get { return this.optimizer.GetType(); }
        }

        /// <summary>
        /// Instance of the optimizer class.
        /// </summary>
        internal IMaterializerMethodOptimizer Optimizer
        {
            get { return this.optimizer; }
        }
    }

    /// <summary>
    /// Interface method optimizers must implement to be used by the Materializer component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Materializer")]
    public interface IMaterializerMethodOptimizer
    {
        /// <summary>
        /// Optimize a method call.
        /// </summary>
        /// <remarks>
        /// Implementations should return the input expression if they are unable to optimize
        /// rather than throwing or returning null.
        /// </remarks>
        /// <param name="fieldNames">Names and order of fields available in the given record.</param>
        /// <param name="recordParameter">Record parameter (of type IDataRecord).</param>
        /// <param name="methodCall">Expression representing the method call.</param>
        /// <returns>Optimized method call.</returns>
        Expression OptimizeMethodCall(ReadOnlyCollection<string> fieldNames, ParameterExpression recordParameter, MethodCallExpression methodCall);
    }
}