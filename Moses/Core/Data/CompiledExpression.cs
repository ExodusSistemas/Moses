
namespace Moses.Data
{
    // Copyright (c) Microsoft Corporation.  All rights reserved.
    // This source code is made available under the terms of the Microsoft Public License (MS-PL)
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides the common boxed version of get.
    /// </summary>
    public abstract class CompiledExpression
    {
        internal abstract LambdaExpression BoxedGet { get; }
    }

    /// <summary>
    /// Represents an expression and its compiled function.
    /// </summary>
    /// <typeparam name="TClass">Class the expression relates to.</typeparam>
    /// <typeparam name="TProperty">Return type of the expression.</typeparam>
    public sealed class CompiledExpression<T, TResult> : CompiledExpression
    {
        private readonly Expression<Func<T, TResult>> expression;
        private readonly Func<T, TResult> function;

        public CompiledExpression()
        {
        }

        public CompiledExpression(Expression<Func<T, TResult>> expression)
        {
            this.expression = expression;
            function = expression.Compile();
        }

        public TResult Evaluate(T instance)
        {
            return function(instance);
        }

        internal override LambdaExpression BoxedGet
        {
            get { return expression; }
        }
    }

   
    /// <summary>
    /// Simple fluent way to access the default translation map.
    /// </summary>
    /// <typeparam name="T">Class the expression uses.</typeparam>
    public static class DefaultTranslationOf<T>
    {
        public static CompiledExpression<T, TResult> Property<TResult>(Expression<Func<T, TResult>> property, Expression<Func<T, TResult>> expression)
        {
            return TranslationMap.defaultMap.Add(property, expression);
        }

        public static IncompletePropertyTranslation<TResult> Property<TResult>(Expression<Func<T, TResult>> property)
        {
            return new IncompletePropertyTranslation<TResult>(property);
        }

        public static TResult Evaluate<TResult>(T instance, MethodBase method)
        {
            var compiledExpression = TranslationMap.defaultMap.Get<T, TResult>(method);
            return compiledExpression.Evaluate(instance);
        }

        public class IncompletePropertyTranslation<TResult>
        {
            private readonly Expression<Func<T, TResult>> property;

            internal IncompletePropertyTranslation(Expression<Func<T, TResult>> property)
            {
                this.property = property;
            }

            public CompiledExpression<T, TResult> Is(Expression<Func<T, TResult>> expression)
            {
                return DefaultTranslationOf<T>.Property(property, expression);
            }
        }
    }

    /// <summary>
    /// Maintains a list of mappings between properties and their compiled expressions.
    /// </summary>
    public class TranslationMap : Dictionary<MemberInfo, CompiledExpression>
    {
        internal static TranslationMap defaultMap = [];

        public CompiledExpression<T, TResult> Get<T, TResult>(MethodBase method)
        {
            var propertyInfo = method.DeclaringType.GetProperty(method.Name.Replace("get_", String.Empty));
            return this[propertyInfo] as CompiledExpression<T, TResult>;
        }

        public void Add<T, TResult>(Expression<Func<T, TResult>> property, CompiledExpression<T, TResult> compiledExpression)
        {
            base.Add(((MemberExpression)property.Body).Member, compiledExpression);
        }

        public CompiledExpression<T, TResult> Add<T, TResult>(Expression<Func<T, TResult>> property, Expression<Func<T, TResult>> expression)
        {
            var compiledExpression = new CompiledExpression<T, TResult>(expression);
            Add(property, compiledExpression);
            return compiledExpression;
        }
    }

    /// <summary>
    /// Extension methods over IQueryable to turn on expression translation via a
    /// specified or default TranslationMap.
    /// </summary>
    public static class ExpressiveExtensions
    {
        public static IQueryable<T> WithTranslations<T>(this IQueryable<T> source)
        {
            return source.Provider.CreateQuery<T>(WithTranslations(source.Expression));
        }

        public static IQueryable<T> WithTranslations<T>(this IQueryable<T> source, TranslationMap map)
        {
            return source.Provider.CreateQuery<T>(WithTranslations(source.Expression, map));
        }

        public static Expression WithTranslations(Expression expression)
        {
            return WithTranslations(expression, TranslationMap.defaultMap);
        }

        public static Expression WithTranslations(Expression expression, TranslationMap map)
        {
            return new TranslatingVisitor(map).Visit(expression);
        }

        private static void EnsureTypeInitialized(Type type)
        {
            try
            {
                // Ensure the static members are accessed class' ctor
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
            catch (TypeInitializationException)
            {

            }
        }

        /// <summary>
        /// Extends the expression visitor to translate properties to expressions
        /// according to the provided translation map.
        /// </summary>
        private class TranslatingVisitor : ExpressionVisitor
        {
            private readonly Stack<KeyValuePair<ParameterExpression, Expression>> bindings = new();
            private readonly TranslationMap map;

            internal TranslatingVisitor(TranslationMap map)
            {
                this.map = map;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                EnsureTypeInitialized(node.Member.DeclaringType);

                if (map.TryGetValue(node.Member, out CompiledExpression cp))
                {
                    return VisitCompiledExpression(cp, node.Expression);
                }

                if (typeof(CompiledExpression).IsAssignableFrom(node.Member.DeclaringType))
                {
                    return VisitCompiledExpression(cp, node.Expression);
                }

                return base.VisitMember(node);
            }

            private Expression VisitCompiledExpression(CompiledExpression ce, Expression expression)
            {
                bindings.Push(new KeyValuePair<ParameterExpression, Expression>(ce.BoxedGet.Parameters.Single(), expression));
                var body = Visit(ce.BoxedGet.Body);
                bindings.Pop();
                return body;
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                var binding = bindings.Where(b => b.Key == p).FirstOrDefault();
                return (binding.Value == null) ? base.VisitParameter(p) : Visit(binding.Value);
            }
        }
    }
   

}
