using System;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Data.Entity;
using Moses.Extensions;
using System.Collections.Generic;

namespace Moses.Web.Mvc.Patterns
{

    public class Repository<TEntity, TType, TDataContext> : IRepository<TEntity, TType>
        where TEntity : class, IEntity<TType>, new()
        where TType : struct, IEquatable<TType>
        where TDataContext : DbContext
    {
        protected IUnitOfWork<TEntity, TType, TDataContext> _unitOfWork;

        public Repository()
        {

        }

        public Repository(IUnitOfWork<TEntity, TType, TDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected DbSet<TEntity> GetTable()
        {
            return _unitOfWork.DbContext.Set<TEntity>();
        }

        public virtual TEntity Get(TType id)
        {
            Expression<Func<TEntity, bool>> predicate = e => e.Id.Equals(id);

            if (typeof(TType) == typeof(int))
            {
                var kid = Convert.ToInt32(id);
                return GetAll().SingleOrDefault(predicate);
            }
            else
            {
                return GetAll().SingleOrDefault(q => q.Id.Equals(id));
            }
        }

        public virtual IQueryable<TEntity> Include(DbSet<TEntity> dbSet)
        {
            var incEnti = _unitOfWork.IncludeEntities();
            var o = _unitOfWork.DbContext.Set<TEntity>().AsQueryable();
            if (incEnti != null)
            {
                foreach (var i in incEnti)
                {
                    o = o.Include(i);
                }
            }
            return o;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Include(this.GetTable()).InContext();
        }

        protected virtual void Validate(TEntity item)
        {
            //validações Gerais
            _unitOfWork.Validate(item);
        }

        public virtual TEntity Copy(TEntity item)
        {
            var t = new TEntity();
            _unitOfWork.Bind(item, t);
            t.Id = default(TType);
            return t;
        }

        public virtual TEntity Copy(TType id)
        {
            return Copy(this.Get(id));
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            var result = _unitOfWork.GetAll();
            if (predicate != null)
            {
                result = result.Where(predicate);
            }
            return result;
        }

        public virtual object Details(TType id)
        {
            return _unitOfWork.Repository.GetSerializableObject(_unitOfWork.Get(id));
        }

        public virtual object GetSerializableObject(TEntity item)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Repository Constrained By A Federated Argument (eg. IdContract)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TDataContext"></typeparam>
    //public class FederatedRepository<TEntity, TType, TDataContext> : Repository<TEntity, TType, TDataContext>, IFederatedRepository<TEntity, TType>
    //    where TEntity : class, IFederatedEntity<TType>, new()
    //    where TType : struct
    //    where TDataContext : DbContext
    //{
    //    protected new IFederatedUnitOfWork<TEntity, TType, TDataContext> _unitOfWork;

    //    public FederatedRepository(IFederatedUnitOfWork<TEntity, TType, TDataContext> unitOfWork) : base(unitOfWork as IUnitOfWork<TEntity, TType, TDataContext>)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public override IQueryable<TEntity> GetAll()
    //    {
    //        return this.GetTable().InContext(this._unitOfWork.MembershipContext);
    //    }

    //    public virtual TEntity Get(TType id, int? idContrato = null)
    //    {
    //        return this.GetAll().SingleOrDefault(q => q.Id.Equals(id));
    //    }

        
    //}


    


}



