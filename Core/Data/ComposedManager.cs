using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Moses.Data
{
    /// <summary>
    /// Snippet para um tipo de Manager que carrega dentro de si numa estrutura de propriedade 
    /// uma entidade Linq
    /// </summary>
    /// <typeparam name="T">Classe Portadora da Propriedade</typeparam>
    /// <typeparam name="K">Classe do DataItem Entity</typeparam>
    /// <typeparam name="L">DataContext</typeparam>
    public abstract class ComposedManager<T, K, L> : Manager<T,L> 
        where T : IComposedItemContainer<K> 
        where L : DbContext
    {
        /// <summary>
        /// Implementação do Método Attach que usa O AttachBase o parâmetro K
        /// </summary>
        /// <param name="item"></param>
        /// <param name="asModified"></param>
        public override void AttachBase(T item, bool asModified)
        {
            AttachBase(item.DataItem, asModified);
        }

        /// <summary>
        /// Habilita o uso do método Attach
        /// </summary>
        /// <example>
        /// ...
        /// {
        ///     Context.SystemItems.Attach(dataItem,asModified);
        /// }
        /// </example>
        /// <param name="dataItem"></param>
        /// <param name="asModified"></param>
        public abstract void AttachBase(K dataItem, bool asModified);
    }
}
