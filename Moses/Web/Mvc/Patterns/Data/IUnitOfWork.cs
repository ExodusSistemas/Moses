using Moses.Extensions;
using Moses.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Moses.Web.Mvc.Controls;

namespace Moses.Web.Mvc.Patterns
{
    #region Base IUnitOfWork interfaces 

    public interface IUnitContext<TEntity, TDataContext> where TEntity : class, new()
        where TDataContext : DbContext
    {
        TDataContext DbContext { get; }
        void SubmitChanges();
        void InitializeEntity(TEntity item);
        void Validate(TEntity item);
    }

    public interface IDataHandler<TEntity, TType, TDataContext> : IUnitContext<TEntity, TDataContext> where TEntity : class, IEntity<TType>, new()
        where TType : struct
        where TDataContext : DbContext
    {
        void Bind(TEntity origem, TEntity destino);
        DbSet<TEntity> Table { get; }
        TEntity Get(TType id);
        IQueryable<TEntity> GetAll();
        TEntity Copy(TType value);
        TEntity Save(TEntity t);
        TType Delete(TType id);
        dynamic Details(TType id);
        List<TType> DeleteAll(string rowIds);
        string GetEntityName();
    }


    public interface IUnitOfWork<TEntity, TType, TDataContext> : IUnitContext<TEntity, TDataContext>, IDataHandler<TEntity, TType, TDataContext> where TEntity : class, IEntity<TType>, new()
        where TType : struct
        where TDataContext : DbContext
    {
        IRepository<TEntity, TType> Repository { get; set; }
        List<System.Linq.Expressions.Expression<Func<TEntity, object>>> IncludeEntities();
    }

    #endregion

    #region Ready-To-Use Interfaces 

    /// <summary>
    /// Grid Unit of Work ready-to-use interface
    /// </summary>
    /// <typeparam name="TEntity">Entity of the UnitOfWork</typeparam>
    /// <typeparam name="TType">Type of the Entity Identifier</typeparam>
    public interface IGridUnitOfWork<TEntity, TType, TDataContext> : IUnitOfWork<TEntity, TType, TDataContext>, IGrid<TEntity>, IAutoComplete
        where TEntity : class, IEntity<TType>, new()
        where TType : struct
        where TDataContext : DbContext
    {

    }

    /// <summary>
    /// Federated Grid Unit of Work ready-to-use interface
    /// </summary>
    /// <typeparam name="TEntityMaster">Entity of the UnitOfWork</typeparam>
    /// <typeparam name="KMaster">Type of the Entity Identifier</typeparam>
    public interface IChildGridUnitOfWork<TEntityMaster, TEntityChild, KMaster, KChild, TDataContext> : IUnitOfWork<TEntityMaster, KMaster, TDataContext>, IGrid<TEntityMaster> where TEntityMaster : class, IEntity<KMaster>, new()
        where KMaster : struct
        where TDataContext : DbContext
    {
        TEntityChild GetChild(KMaster masterId, KChild id);
        IQueryable<TEntityChild> GetAll(KMaster masterId);
    }



    #endregion

    #region UnitOfWork Interfaces Plugins

    public interface IGrid<T>
        where T: class, new()
    {
        GridControl<T> GetGrid();
    }

    public interface IAutoComplete
    {
        IEnumerable<object> GetSerializableList(System.Collections.IEnumerable list);
    }

    #endregion
}