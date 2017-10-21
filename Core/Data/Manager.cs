using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Moses.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public abstract class Manager<T, K> : ManagerBase<K> where K : DbContext
    {
        /// <summary>
        /// Construtor padrão da classe. Cria um novo DataContext e retorna a referência do Manager
        /// </summary>
        public Manager() : base()
        {
           
        }

        /// <summary>
        /// Construtor da classe que recebe DataContext e retorna a referência do Manager com a referida referência
        /// </summary>
        public Manager(K context) : base(context)
        {
           
        }


        public abstract IQueryable<T> GetAll();

        public abstract T Get(int itemId);

        public abstract T Create(T item);

        public void Delete(T item, bool attatch)
        {
            if (attatch) Attach(item);

            DeleteBase(item);
        }

        public void Delete(T item)
        {
            Delete(item, true);
        }

        public void Attach(T item)
        {
            AttachBase(item, false);
        }

        public abstract void AttachBase(T item, bool asModified);

        public abstract void DeleteBase(T item);

        public void UpdateAsModified(T item)
        {
            AttachBase(item, true);
        }
    }
}
