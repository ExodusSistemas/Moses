using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Data.Objects;
using Moses.Core.Data.EFExtensions;

namespace Moses.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class EntitySet<T> : ObjectQuery<T>
    {
        private readonly EntitySet metadata;

        /// <summary>
        /// Constructs an entity set query.
        /// </summary>
        /// <param name="context">Context to which entity set is bound.</param>
        public EntitySet(ObjectContext context)
            : this(context, (string)null, (string)null, MergeOption.AppendOnly) { }

        /// <summary>
        /// Construct an entity set query.
        /// </summary>
        /// <param name="context">Context to which entity set is bound.</param>
        /// <param name="entitySetName">Name of the entity set. Must be consistent with the
        /// entity set element type.</param>
        public EntitySet(ObjectContext context, string entitySetName)
            : this(context, (string)null, entitySetName, MergeOption.AppendOnly) { }

        /// <summary>
        /// Construct an entity set query.
        /// </summary>
        /// <param name="context">Context to which entity set is bound.</param>
        /// <param name="entitySetName">Name of the entity set. Must be consistent with the
        /// entity set element type.</param>
        /// <param name="mergeOption">Merge option to use for the query.</param>
        public EntitySet(ObjectContext context, string entitySetName, MergeOption mergeOption)
            : this(context, (string)null, entitySetName, mergeOption) { }

        /// <summary>
        /// Construct an entity set query.
        /// </summary>
        /// <param name="context">Context to which entity set is bound.</param>
        /// <param name="containerName">Name of the entity set's container.</param>
        /// <param name="entitySetName">Name of the entity set. Must be consistent with the
        /// entity set element type.</param>
        public EntitySet(ObjectContext context, string containerName, string entitySetName)
            : this(context, containerName, entitySetName, MergeOption.AppendOnly) { }

        /// <summary>
        /// Construct an entity set query.
        /// </summary>
        /// <param name="context">Context to which entity set is bound.</param>
        /// <param name="containerName">Name of the entity set's container.</param>
        /// <param name="entitySetName">Name of the entity set. Must be consistent with the
        /// entity set element type.</param>
        /// <param name="mergeOption">Merge option to use for the query.</param>
        public EntitySet(ObjectContext context, string containerName, string entitySetName, MergeOption mergeOption)
            : this(context, GetEntitySet(context, containerName, entitySetName), mergeOption) { }

        private EntitySet(ObjectContext context, EntitySet entitySet, MergeOption mergeOption)
            : base(GetCommandText(entitySet), context, mergeOption)
        {
            this.metadata = entitySet;
        }

        /// <summary>
        /// Gets metadata for the EntitySet.
        /// </summary>
        public EntitySet Metadata
        {
            get { return this.metadata; }
        }

        /// <summary>
        /// Gets qualified entity set name. Used to associate entities with the set.
        /// </summary>
        private string QualifiedEntitySetName
        {
            get { return this.Metadata.EntityContainer.Name + "." + this.Metadata.Name; }
        }

        /// <summary>
        /// Tracks a new entity for insertion when ObjectContext.SaveChanges is called.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        public void InsertOnSaveChanges(T entity)
        {
            Utility.CheckArgumentNotNull(((object)entity), "entity");

            this.Context.AddObject(this.QualifiedEntitySetName, entity);
        }

        /// <summary>
        /// Removes an entity from the EntitySet. The entity will be deleted
        /// when ObjectContext.SaveChanges is called.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public void DeleteOnSaveChanges(T entity)
        {
            Utility.CheckArgumentNotNull(((object)entity), "entity");

            // check that the entity actually exists first
            ObjectStateEntry stateEntry;

            if (!this.Context.ObjectStateManager.TryGetObjectStateEntry(entity, out stateEntry))
            {
                throw new ArgumentException(Messages.UntrackedEntity, "entity");
            }

            // now check that the entity belongs to the current entity set
            if (this.metadata != stateEntry.EntitySet)
            {
                throw new ArgumentException(Messages.DeletingFromWrongSet, "entity");
            }

            this.Context.DeleteObject(entity);
        }

        /// <summary>
        /// Attaches an existing entity to the current entity set.
        /// </summary>
        /// <param name="entity">Entity to attach.</param>
        public void Attach(T entity)
        {
            Utility.CheckArgumentNotNull(((object)entity), "entity");

            this.Context.AttachTo(this.QualifiedEntitySetName, entity);
        }

        /// <summary>
        /// Attaches the given entity or returns existing entity with the same key.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity to attach.</param>
        /// <returns>Input entity or existing entity with the same key.</returns>
        public TEntity FindOrAttach<TEntity>(TEntity entity)
            where TEntity : T
        {
            if (null == (object)entity)
            {
                return default(TEntity);
            }
            EntityKey entityKey = this.Context.CreateEntityKey(this.QualifiedEntitySetName, entity);
            ObjectStateEntry existingStateEntry;
            if (this.Context.ObjectStateManager.TryGetObjectStateEntry(entityKey, out existingStateEntry) &&
                null != existingStateEntry.Entity) // A proxy entry may exist for the entity instance
            {
                try
                {
                    return (TEntity)existingStateEntry.Entity;
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException(Messages.AttachedEntityHasWrongType);
                }
            }
            else
            {
                Attach(entity);
                return entity;
            }
        }

        /// <summary>
        /// Gets all members of the EntitySet that are currently in memory.
        /// </summary>
        /// <returns>All tracked members of the EntitySet</returns>
        public IEnumerable<T> GetTrackedEntities()
        {
            return GetTrackedEntities(~EntityState.Detached);
        }

        /// <summary>
        /// Gets all members of the EntitySet that are currently in memory
        /// with the given state(s).
        /// </summary>
        /// <param name="state">Entity state flags.</param>
        /// <returns>Tracked members of the EntitySet in the given state.</returns>
        public IEnumerable<T> GetTrackedEntities(EntityState state)
        {
            return this.Context.ObjectStateManager.GetObjectStateEntries(state)
                .Where(IsMemberOfEntitySet).Select(e => e.Entity).Cast<T>();
        }

        private bool IsMemberOfEntitySet(ObjectStateEntry entry)
        {
            return !entry.IsRelationship // must be an entity
                && null != entry.Entity // must not be a key entry
                && entry.EntitySet == this.metadata; // must belong to the current set
        }

        private static EntitySet GetEntitySet(ObjectContext context, string containerName, string entitySetName)
        {
            Utility.CheckArgumentNotNull(context, "context");

            // if no container is specified, use the default for the context
            containerName = string.IsNullOrEmpty(containerName) ? context.DefaultContainerName : containerName;
            Utility.CheckArgumentNotNullOrEmpty(containerName, "containerName");

            // ensure the entity container exists
            EntityContainer container;
            if (!context.MetadataWorkspace.TryGetEntityContainer(containerName,
                DataSpace.CSpace, out container))
            {
                throw new ArgumentException(String.Format(Messages.Culture, Messages.UnknownEntityContainer, containerName), "containerName");
            }

            EntitySet entitySet;

            if (string.IsNullOrEmpty(entitySetName))
            {
                // if no entity set is specified, try to find a single entity set taking this type
                entitySet = GetDefaultEntitySet(context, container);
            }
            else if (!container.TryGetEntitySetByName(entitySetName, false, out entitySet))
            {
                // ensure the requested entity set exists
                throw new ArgumentException(String.Format(Messages.Culture, Messages.UnknownEntitySet, entitySetName), "entitySetName");
            }

            return entitySet;
        }

        private static EntitySet GetDefaultEntitySet(ObjectContext context, EntityContainer container)
        {
            EntitySet entitySet;

            // register the assembly
            context.MetadataWorkspace.LoadFromAssembly(typeof(T).Assembly);

            // try to find a single entity set accepting the given type
            StructuralType edmType = GetEdmType(context.MetadataWorkspace);
            var candidates = from candidate in container.BaseEntitySets.OfType<EntitySet>()
                             where IsSubtype(candidate.ElementType, edmType)
                             select candidate;
            using (var candidateEnumerator = candidates.GetEnumerator())
            {
                if (candidateEnumerator.MoveNext())
                {
                    entitySet = candidateEnumerator.Current;
                    if (candidateEnumerator.MoveNext())
                    {
                        // more than one match
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Messages.AmbiguousEntitySet, typeof(T)));
                    }
                }
                else
                {
                    // no matches
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Messages.NoEntitySet, typeof(T)));
                }
            }
            return entitySet;
        }

        private static bool IsSubtype(EdmType baseType, EdmType derivedType)
        {
            while (derivedType != null)
            {
                if (derivedType == baseType)
                {
                    return true;
                }
                derivedType = derivedType.BaseType;
            }
            return false;
        }

        private static StructuralType GetEdmType(MetadataWorkspace workspace)
        {
            StructuralType objectSpaceType;
            workspace.LoadFromAssembly(typeof(T).Assembly);
            if (!workspace.TryGetItem<StructuralType>(typeof(T).FullName, DataSpace.OSpace, out objectSpaceType))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Messages.UnableToFindMetadataForType, typeof(T)));
            }
            StructuralType edmSpaceType = workspace.GetEdmSpaceType(objectSpaceType);
            return edmSpaceType;
        }

        private static string GetCommandText(EntitySet entitySet)
        {
            Utility.CheckArgumentNotNull(entitySet, "entitySet");

            // to query an entity set, simply name it
            string containerName = entitySet.EntityContainer.Name;
            string entitySetName = entitySet.Name;

            // quote the identifiers
            return QuoteIdentifier(containerName) + "." + QuoteIdentifier(entitySetName);
        }

        private static string QuoteIdentifier(string identifier)
        {
            Utility.CheckArgumentNotNullOrEmpty(identifier, "identifier");
            return "[" + identifier.Replace("]", "]]") + "]";
        }
    }
}
