using LiveDirectorySyncEngineLogic.Generic.Model;
using NDatabase;
using NDatabase.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess.NDatabaseImplementation
{
    /// <summary>
    /// Implementation of NDatabase
    /// http://ndatabase.wixsite.com/home
    /// 
    /// </summary>
    public class NDatabaseConnection : IDBConnection
    {
        public string ConnectionString { get; }
        public IOdb Odb { get; private set; }
        public bool IsOpen { get; private set; }

        public NDatabaseConnection(string dbFileName)
        {
            ConnectionString = dbFileName;
            Open();
        }

        public void Open()
        {
            Odb = OdbFactory.Open(ConnectionString);
            IsOpen = true;
        }

        public void Close()
        {
            if (Odb is IDisposable disposable)
            {
                disposable.Dispose();
            }
            Odb = null;
            IsOpen = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && Odb != null)
            {
                Close();
            }
        }

        public void Store<T>(T obj) where T : class
        {
            Odb.Store(obj);
        }

        public IList<T> GetAll<T>() where T : class
        {
            IObjectSet<T> objects = Odb.Query<T>().Execute<T>();
            return objects.ToList();
        }

        public T Get<T>(int id) where T : class, IIdentifier
        {
            IObjectSet<T> objects = Odb.Query<T>().Execute<T>();
            return objects.FirstOrDefault(a => a.Id == id);
        }
    }
}
