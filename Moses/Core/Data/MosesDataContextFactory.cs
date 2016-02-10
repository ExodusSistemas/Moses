using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Moses.Data
{
    public class DataContextFactory<T> where T : DataContext , new()
    {
        public static DataContextFactory<T> _factory = null;
        private T _dataContext = null;

        public static T GetDataContext()
        {
            if (_factory == null)
            {
                Initialize(ref _factory);
                return _factory._dataContext;
            }
            return _factory._dataContext;
            
        }

        public static void Start()
        {
            Initialize(ref _factory);
        }

        private static void Initialize(ref DataContextFactory<T> factory)
        {
            factory = new DataContextFactory<T>();
            factory._dataContext = new T();
        }

        

    }

}
