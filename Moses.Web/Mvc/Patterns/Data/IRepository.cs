using System;
using System.Linq;
using System.Linq.Expressions;
using Trirand.Web.Mvc;

namespace Moses.Web.Mvc.Patterns
{

    #region Repository Interfaces

    public interface IRepository<T, K> where T : class, IEntity<K>, new()
        where K : struct
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        T Copy(T item);
        T Copy(K id);
        T Get(K id);
        object Details(K id);
        object GetSerializableObject(T item);
    }

    #endregion


}