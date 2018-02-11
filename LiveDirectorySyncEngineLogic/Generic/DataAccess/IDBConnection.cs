using LiveDirectorySyncEngineLogic.Generic.Model;
using LiveDirectorySyncEngineLogic.Generic.Model;
using System;
using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess
{
    public interface IDBConnection : IDisposable
    {
        string ConnectionString { get; }
        void Open();
        void Close();

        void Store<T>(T obj) where T : class;
        IList<T> GetAll<T>() where T : class;
        T Get<T>(int id) where T : class, IIdentifier;
    }
}
