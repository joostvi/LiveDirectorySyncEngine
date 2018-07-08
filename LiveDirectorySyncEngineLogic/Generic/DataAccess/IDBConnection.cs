using LiveDirectorySyncEngineLogic.Generic.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess
{
    public interface IDBConnection : IDisposable
    {
        string ConnectionString { get; }
        void Close();
        void Open();

        void Store<T>(T obj) where T : class;
        IList<T> GetAll<T>() where T : class;
        T Get<T>(int id) where T : class, IIdentifier;
    }

    public interface IDBRepository
    {
        void Store<T>(T obj) where T : class;
        IList<T> GetAll<T>() where T : class;
        T Get<T>(int id) where T : class, IIdentifier;
    }
}
