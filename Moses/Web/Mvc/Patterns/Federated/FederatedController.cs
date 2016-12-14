using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Moses.Web.Mvc.Patterns
{
    //public class BasicFederatedController<TEntity, TUnitOfWork, TDataContext> : BasicController<TEntity, int, TUnitOfWork, TDataContext, ViewModel<TEntity, int>>
    //    where TEntity : class, IFederatedEntity<int>, IEntity<int>, new()
    //    where TUnitOfWork : class, IUnitOfWork<TEntity, int, TDataContext>, IFederatedUnitOfWork<TEntity, int, TDataContext>, IGrid<TEntity>, new()
    //    where TDataContext : DbContext

    //{

    //}


    //public class BasicFederatedController<TEntity, TType, TUnitOfWork, TDataContext> : BasicController<TEntity, TType, TUnitOfWork, TDataContext, ViewModel<TEntity, TType>>
    //    where TEntity : class, IFederatedEntity<TType>, IEntity<TType>, new()
    //    where TUnitOfWork : class, IUnitOfWork<TEntity, TType, TDataContext>, IFederatedUnitOfWork<TEntity, TType, TDataContext>, IGrid<TEntity>, new()
    //    where TDataContext : DbContext
    //    where TType : struct, IEquatable<TType>
    //{

    //}


    //public class BasicFederatedController<TEntity, TType, TUnitOfWork, TDataContext, TViewModel> : BasicController<TEntity, TType, TUnitOfWork, TDataContext, TViewModel>
    //    where TEntity : class, IFederatedEntity<TType>, IEntity<TType>, new()
    //    where TUnitOfWork : class, IUnitOfWork<TEntity, TType, TDataContext>, IFederatedUnitOfWork<TEntity, TType, TDataContext>, IGrid<TEntity>, new()
    //    where TViewModel : ViewModel<TEntity, TType>, new()
    //    where TDataContext : DbContext
    //    where TType : struct, IEquatable<TType>
    //{
    //    private TUnitOfWork _manager = null;
    //    public override TUnitOfWork Manager
    //    {
    //        get
    //        {
    //            if (_manager == null)
    //            {
    //                _manager = new TUnitOfWork();
    //                _manager.MembershipContext = this.MembershipContext;
    //            }
    //            return _manager;
    //        }
    //    }
    //}
}