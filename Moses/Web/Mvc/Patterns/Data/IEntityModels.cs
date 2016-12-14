using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trirand.Web.Mvc;

namespace Moses.Web.Mvc.Patterns
{
    public interface IEntity<K> where K : struct
    {
        K Id { get; set; }
    }

    /// <summary>
    /// Interface for define a Child Entity
    /// </summary>
    /// <typeparam name="K">Type of the identifier</typeparam>
    /// <typeparam name="TEntityParent">Parent Entity Type</typeparam>
    /// <typeparam name="J">Parent Entity Id Type</typeparam>
    public interface IChildEntity<K, TEntityParent, J> : IEntity<K>
        where K : struct
        where TEntityParent : IEntity<J>
        where J : struct

    {
        TEntityParent Parent { get; }
        J ParentId { get; set; }
    }

    public interface IDeletable<TEntity>
    {
        bool IsDeleted { get; set; }
        IQueryable<TEntity> FilterDeleted(IQueryable<TEntity> query);
    }

    public interface IFederated<TEntity>
    {
        int IdContrato { get; set; }
        IQueryable<TEntity> FilterFederated(IQueryable<TEntity> query);
    }

    public interface IAuditable
    {
        DateTime DatCriacao { get; set; }
        DateTime DatUltimaAlteracao { get; set; }
        Guid? IdUserCreator { get; set; }
        Guid? IdUserEditor { get; set; }

    }

}
