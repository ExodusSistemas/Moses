using Moses.Security;
using Moses.Web.Mvc.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses.Extensions
{
    
    public static class EntityHelper
    {
        public static Dictionary<Type, EntityHelperProps> ContextCache = new Dictionary<Type, EntityHelperProps>();
        
        public class EntityHelperProps
        {
            public bool IsDeletable { get; set; }
            public bool IsFederated { get; set; }
        }

        public static IQueryable<TEntity> InContext<TEntity>(this IQueryable<TEntity> query, MembershipContext context = null, bool includeDeleted = false)
            where TEntity : class , new()
        {
            bool hasContext = context != null;

            var props = GetCachedProps<TEntity>();

            if (!hasContext && !props.IsDeletable && !props.IsFederated)
                return query;

            if (!hasContext && props.IsFederated)
                throw new ApplicationException("O context não está definido");

            if (props.IsDeletable)
            {
                //System.Linq.Expressions.Expression<Func<dynamic, bool>> predicate = e => ((IFederated)e).IdContrato.Equals(false);
                IDeletable<TEntity> t = (IDeletable<TEntity>)new TEntity(); //create and use interface object to apply filter
                query = t.FilterDeleted(query);
            }

            if (props.IsFederated)
            {
                IFederated<TEntity> t = (IFederated<TEntity>)new TEntity(); //create and use interface object to apply filter
                query = t.FilterFederated(query);
            }

            return query;
        }

        private static EntityHelperProps GetCachedProps<TEntity>() where TEntity : class
        {

//#if RELEASE
//            if (!ContextCache.ContainsKey(typeof(TEntity)))
//            {
//                ContextCache.Add(typeof(TEntity), new EntityHelperProps()
//                {
//                    IsDeletable = typeof(TEntity).GetInterfaces().OfType<IDeletable>().Count() > 0,
//                    IsFederated = typeof(TEntity).GetInterfaces().OfType<IFederated>().Count() > 0,
//                });
//            }



//            return ContextCache[typeof(TEntity)];

//#endif

//#if DEBUG
            var interfaces = typeof(TEntity).GetInterfaces();
            return new EntityHelperProps()
            {
                IsDeletable = interfaces.Where(q => q.Name == typeof(IDeletable<>).Name).Count() > 0,
                IsFederated = interfaces.Where(q => q.Name == typeof(IFederated<>).Name).Count() > 0,
            };
//#endif

        }

        //public static IQueryable<TEntity> InContext<TEntity>(this IQueryable<TEntity> query, MembershipContext context, bool includeDeleted = false)
        //    where TEntity : IFederated, IDeletable
        //{
        //    if (includeDeleted)
        //    {
        //        return query.Where(q => q.IdContrato == context.Contract.Id);
        //    }
        //    else
        //    {
        //        return query.Where(q => q.IdContrato == context.Contract.Id && q.IsDeleted == false);
        //    }
        //}
    }

}