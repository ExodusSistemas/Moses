using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using Moses.Core.Data.EFExtensions;
using System.Data.Entity.Core;

namespace Moses.Data{
    /// <summary>
    /// Extensions supporting easy retrieval and setting of reference key values. Currently, the EntityReference
    /// class requires metadata for a key but does not directly expose that metadata. This class exposes the 
    /// keys as pure values.
    /// </summary>
    /// <example>
    /// <code>
    /// // existing pattern
    /// product.CategoryReference.EntityKey = new EntityKey("EntityContainer.Categories", new EntityKeyMember("CategoryID", 1), new EntityKeyMember("DivisionID", 1))); 
    /// 
    /// // new pattern
    /// product.CategoryReference.SetKey(1, 1);
    /// </code>
    /// </example>
    public static class EntityReferenceExtensions
    {
        /// <summary>
        /// Gets key value for a non-compound reference key (e.g. one foreign key component).
        /// </summary>
        /// <typeparam name="T">EntityReference element type</typeparam>
        /// <param name="entityReference">Entity reference.</param>
        /// <returns>Key value from entity reference.</returns>
        public static object GetKey<T>(this EntityReference<T> entityReference)
            where T : class, IEntityWithRelationships
        {
            Utility.CheckArgumentNotNull(entityReference, "entityReference");
            EntityKey entityKey = entityReference.EntityKey;
            if (null == entityKey)
            {
                if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count != 1)
                {
                    throw new InvalidOperationException(Messages.SimpleKeyOnly);
                }
                return null;
            }
            var entityKeyValues = entityKey.EntityKeyValues;
            if (entityKeyValues.Length != 1)
            {
                throw new InvalidOperationException(Messages.SimpleKeyOnly);
            }
            return entityKeyValues[0].Value;
        }

        /// <summary>
        /// Gets a component of a key value for a (potentially compound) reference key.
        /// </summary>
        /// <typeparam name="T">EntityReference element type</typeparam>
        /// <param name="entityReference">Entity reference.</param>
        /// <param name="keyOrdinal">Index of the key component (with respect to the element type's
        /// EntityType.KeyMembers).</param>
        /// <returns>Key component value from entity reference.</returns>
        public static object GetKey<T>(this EntityReference<T> entityReference, int keyOrdinal)
            where T : class, IEntityWithRelationships
        {
            Utility.CheckArgumentNotNull(entityReference, "entityReference");
            if (keyOrdinal < 0)
            {
                throw new ArgumentOutOfRangeException("keyOrdinal");
            }
            EntityKey entityKey = entityReference.EntityKey;
            if (null == entityKey)
            {
                if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count <= keyOrdinal)
                {
                    throw new ArgumentOutOfRangeException("keyOrdinal");
                }
                return null;
            }
            if (entityKey.EntityKeyValues.Length <= keyOrdinal)
            {
                throw new ArgumentOutOfRangeException("keyOrdinal");
            }
            return entityKey.EntityKeyValues[keyOrdinal].Value;
        }

        /// <summary>
        /// Sets reference key values.
        /// </summary>
        /// <typeparam name="T">EntityReference element type</typeparam>
        /// <param name="entityReference">Entity reference.</param>
        /// <param name="keyValues">Components of the key (aligned with the element type EntityType.KeyMembers)</param>
        public static void SetKey<T>(this EntityReference<T> entityReference, params object[] keyValues)
            where T : class, IEntityWithRelationships
        {
            Utility.CheckArgumentNotNull(entityReference, "entityReference");

            // if null keyValues given, clear the entity key
            if (null == keyValues)
            {
                entityReference.EntityKey = null;
            }

            IEnumerable<string> keyComponentNames;
            int expectedKeyComponentCount;
            string entitySetName;

            if (null == entityReference.EntityKey)
            {
                // if there is no existing key, retrieve metadata through reflection
                EntitySet targetEntitySet = entityReference.GetTargetEntitySet();
                keyComponentNames = targetEntitySet.ElementType.KeyMembers.Select(m => m.Name);
                expectedKeyComponentCount = targetEntitySet.ElementType.KeyMembers.Count;
                entitySetName = targetEntitySet.EntityContainer.Name + "." + targetEntitySet.Name;
            }
            else
            {
                // if there is an existing key, just borrow its metadata
                EntityKey existingKey = entityReference.EntityKey;
                keyComponentNames = existingKey.EntityKeyValues.Select(v => v.Key);
                expectedKeyComponentCount = existingKey.EntityKeyValues.Length;
                entitySetName = existingKey.EntityContainerName + "." + existingKey.EntitySetName;
            }

            // check that the correct number of key values is given
            if (keyValues != null && expectedKeyComponentCount != keyValues.Length)
            {
                throw new ArgumentException(Messages.UnexpectedKeyCount, "keyValues");
            }

            // check if there are any null key components (if so, the entire key is assumed
            // to be null)
            if (keyValues == null || keyValues.Any(v => null == v))
            {
                entityReference.EntityKey = null;
            }
            else
            {
                // create a new entity key with the given key component names and key component values
                EntityKey entityKey = new EntityKey(entitySetName, keyComponentNames.Zip(keyValues,
                    (name, value) => new EntityKeyMember(name, value)));
                entityReference.EntityKey = entityKey;
            }
        }

        /// <summary>
        /// Uses internal API to access metadata for related end target.
        /// </summary>
        /// <param name="relatedEnd">Related end.</param>
        /// <returns>Entity set targeted by the related end.</returns>
        public static EntitySet GetTargetEntitySet(this RelatedEnd relatedEnd)
        {
            Utility.CheckArgumentNotNull(relatedEnd, "relatedEnd");

            AssociationSet associationSet = (AssociationSet)relatedEnd.RelationshipSet;

            if (null == associationSet)
            {
                throw new InvalidOperationException(Messages.CannotDetermineMetadataForRelatedEnd);
            }

            return associationSet.AssociationSetEnds[relatedEnd.TargetRoleName].EntitySet;
        }
    }
}