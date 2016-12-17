using Moses.Extensions;
using Moses.Web.Mvc.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses.Web.Mvc.Patterns
{
    public class ViewModel<TEntity> : ViewModel<TEntity, int>, IModel
        where TEntity : class, IEntity<int>, new()
    {
        public override bool IsEdit
        {
            get
            {
                return Item != null ? Item.Id > 0 : false;
            }
        }

    }

    public class ViewModel<TEntity, TType> : BaseViewModel, IModel
       where TEntity : class, IEntity<TType>, new()
       where TType : struct
    {
        TEntity _item = null;
        public TEntity Item
        {
            get
            {
                return _item ?? new TEntity();
            }
            set
            {
                _item = value;
            }
        }

        public override bool IsEdit
        {
            get
            {
                return _item?.Id.Equals(default(TType)) ?? false;
            }
        }

        public IQueryable<TEntity> List { get; set; }

        public GridControl<TEntity> Grid { get; set; }
    }

    public abstract class BaseViewModel : IModel
    {
        #region IModel Members

        public string Message
        {
            get;
            set;
        }

        public abstract bool IsEdit { get; }

        public UserOperation? Operation { get; set; }

        #endregion

    }

    public interface IdContainer<T>
    {
        T Id { get; set; }
    }

    public interface IModel
    {
        bool IsEdit { get; }

        string Message { get; set; }
    }

    public enum UserOperation
    {
        Add,
        Edit,
        MultiEdit,
        Delete,
        Reload,
    }
}