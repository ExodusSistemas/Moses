using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Moses.Data
{
    
    /// <summary>
    /// Classe Básica de representação da estrutura de acesso a dados no Moses 3.0
    /// </summary>
    public abstract class ManagerBase<T> where T : DataContext
    {
        T _db;

        /// <summary>
        /// Construtor padrão da classe. Cria um novo DataContext e retorna a referência do Manager
        /// </summary>
        public ManagerBase()
        {
            _db = GetNewDataContext();
        }

        /// <summary>
        /// Construtor da classe que recebe DataContext e retorna a referência do Manager com a referida referência
        /// </summary>
        public ManagerBase(T context)
        {
            _db = context;
        }

        /// <summary>
        /// Executa o SubmitChanges do DataContext controlado pelo manager
        /// </summary>
        public void SubmitChanges()
        {
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Referência para o DataContext
        /// </summary>
        public virtual T Context
        {
            get
            {
                if (_db == null) _db = GetNewDataContext();
                return _db;
            }
        }

        /// <summary>
        /// Deve retornar o DataContext para ser uDsado internamente na Criação do DataContext dentro do construtor
        /// da classe
        /// </summary>
        /// <returns></returns>
        public abstract T GetNewDataContext();
    }
}
