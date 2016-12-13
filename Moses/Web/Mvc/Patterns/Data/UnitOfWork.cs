using System;
using System.Collections.Generic;
using System.Linq;
using Moses.Web.Mvc.Patterns;
using Trirand.Web.Mvc;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using Moses.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Helpers;
using Moses.Extensions;
using Microsoft.Owin.Host.SystemWeb;

namespace Moses.Web.Mvc.Patterns
{

    public abstract class UnitOfWork<TEntity, TType, TDataContext> :  IUnitOfWork<TEntity, TType, TDataContext>, IAutoComplete where TEntity : class, IEntity<TType>, new()
        where TType : struct , IEquatable<TType>
        where TDataContext : DbContext, new()
    {

        public MembershipContext MembershipContext { get; set; }
        protected TDataContext _db;
        protected bool _noContext = false; //lock for no context use, put true to unlock on derived class

        public IRepository<TEntity, TType> Repository { get; set; }

        public TDataContext DbContext
        {
            get { return _db; }
        }

        public UnitOfWork() :
            this(null)
        {
            
        }

        public virtual string GetEntityName()
        {
            return typeof(TEntity).Name;
        }

        public UnitOfWork(TDataContext db)
        {
            _db = db ?? System.Web.HttpContext.Current.GetOwinContext().Get<TDataContext>();
            Repository = new Repository<TEntity, TType, TDataContext>(this);
        }

        /// <summary>
        /// Initializes the repository 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void InitializeRepository()
        {

        }

        /// <summary>
        /// Data Source for UnitOfWork
        /// </summary>
        public virtual DbSet<TEntity> Table
        {
            get
            {
                return _db.Set<TEntity>();
            }
        }

        /// <summary>
        /// Initializes Entity Scope. 
        /// </summary>
        /// <remarks>
        /// Set default Entity values on basic abstract implementation
        /// </remarks>
        /// <returns></returns>
        protected virtual TEntity InitializeEntity()
        {
            TEntity destino = new TEntity();

            if (typeof(TEntity) is IFederated<TEntity>)
            {
                ((IFederated<TEntity>)destino).IdContrato = this.MembershipContext.Contract.Id.GetValueOrDefault(); ;
            }

            if (typeof(TEntity) is IDeletable<TEntity>)
            {
                ((IDeletable<TEntity>)destino).IsDeleted = true;
            }

            this.InitializeEntity(destino);
            return destino;
        }

        public TEntity Save(TEntity item)
        {
            TEntity destino;
            if (item.Id.Equals(default(TType)))
            {
                destino = this.InitializeEntity();

                //inicializações adicionais
                Table.Add(destino);
            }
            else
                destino = Repository.Get(item.Id);

            this.Bind(item, destino);
            this.Validate(destino);

            return destino;
        }

        public virtual void Bind(TEntity origem, TEntity destino)
        {
            throw new MosesApplicationException("Operação de Bind não foi implementada no UnitOfWork");
        }

        public void SubmitChanges()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbuex)
            {
                throw new MosesApplicationException("Erro ao fazer a atualização: " + dbuex.InnerException.ToString(), dbuex);
            }
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

        public virtual IQueryable<TEntity> GetAll()
        {
            return Constrained(Repository.GetAll().InContext());
        }

        public virtual IQueryable<TEntity> Constrained(IQueryable<TEntity> entity)
        {
            return entity;
        }

        public virtual dynamic Details(TType id)
        {
            return Repository.Details(id);
        }

        public virtual TEntity Copy(TEntity item)
        {
            var t = new TEntity();
            this.Bind(item, t);
            t.Id = default(TType);
            return t;
        }

        public TEntity Copy(TType value)
        {
            return this.Copy(Get(value));
        }

        public virtual TType Delete(TType id)
        {
            return Delete(id, null as TEntity);
        }

        public virtual TType Delete(TType? id = null, TEntity item = null)
        {
            if (id != null)
                item = Table.Single(q => q.Id.Equals(id));

            if (item.Id.Equals(default(TType))) throw new MosesApplicationException("O Id do contrato tem que ser diferente de zero");

            if (typeof(TEntity) is IFederated<TEntity>)
            {
                if ( ((IFederated<TEntity>)item).IdContrato == 0) throw new MosesApplicationException("Contrato inválido");
            }

            if (typeof(TEntity) is IDeletable<TEntity>)
            {
                ((IDeletable<TEntity>)item).IsDeleted = true;
            }

            return item.Id;
        }

        public virtual List<TType> DeleteAll(string rowIds)
        {
            List<TType> result = new List<TType>();

            foreach (var s in rowIds.Split(','))
            {
                TType i = default(TType);
                if (this.ParseKey(s, out i))
                {
                    this.Delete(i);
                    result.Add(i);
                }
            }

            return result;
        }

        public virtual void InitializeEntity(TEntity item)
        {

        }

        public virtual void Validate(TEntity item)
        {

        }

        public virtual object GetSerializableObject(TEntity item)
        {
            return item;
        }

        public IEnumerable<object> GetSerializableList(System.Collections.IEnumerable list)
        {
            foreach (var l in list)
            {
                yield return GetSerializableObject((TEntity)l);
            }

        }

        public virtual bool ParseKey(string stringKey, out TType i)
        {
            //cobre os principais
            TType o = (TType)Convert.ChangeType(stringKey, typeof(TType));
            i = o;
            return true;
        }

        public virtual TType Delete(TType id = default(TType), TEntity item = null)
        {
            var defaultK = default(TType);
            if (!id.Equals(defaultK))
            {
                Expression<Func<TEntity, bool>> predicate = e => e.Id.Equals(id);
                item = Table.Single(predicate);
            }

            if (((IEquatable<TType>)item.Id).Equals(default(TType))) throw new MosesApplicationException("O Id tem que ser diferente de zero");

            if (item is IDeletable<TEntity>)
            {
                ((IDeletable<TEntity>)item).IsDeleted = true;
            }
            else
            {
                Table.Remove(item);
            }

            return item.Id;
        }

        public virtual List<Expression<Func<TEntity, object>>> IncludeEntities()
        {
            return new List<Expression<Func<TEntity, object>>>();
        }
    }

    #region  Unit Of Work Set

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">Entity for the unit of work to Handle</typeparam>
    public class UnitOfWork<TEntity, TDataContext> : UnitOfWork<TEntity, int, TDataContext>
        where TEntity : class, IEntity<int>, new()
        where TDataContext :  DbContext, new()
    {
        public UnitOfWork() :
            base(null)
        {

        }

        public UnitOfWork(TDataContext db) : base(db)
        {

        }

    }

    #endregion

}


