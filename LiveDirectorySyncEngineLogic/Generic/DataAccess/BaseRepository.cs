using LiveDirectorySyncEngineLogic.Generic.Model;
using System;
using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess
{
    public abstract class BaseRepository<T> : IDBRepository<T> where T : class, IIdentifier
    {
        private readonly IDBConnection _Connection;

        protected BaseRepository(IDBConnection connection)
        {
            _Connection = connection ?? throw new NullReferenceException(nameof(connection) + " is missing a value!");
  
        }

        public IList<T> GetAll() 
        {
            return _Connection.GetAll<T>();
        }

        public T Get(int id)
        {
            return _Connection.Get<T>(id);
        }

        public void Store(T obj)
        {
            _Connection.Store(obj);
        }
    }
}
