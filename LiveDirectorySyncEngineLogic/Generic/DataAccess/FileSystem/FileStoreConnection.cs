﻿using LiveDirectorySyncEngineLogic.Generic.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess.FileSystem
{
    public class FileStoreConnection : IDBConnection
    {

        private DbConnectionStringBuilder DbConnectionStringBuilder { get; }
        /// <summary>
        /// Filename of the file where the data is stored.
        /// </summary>
        /// <param name="connectionString"></param>
        public FileStoreConnection(string connectionString)
        {
            DbConnectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public void Close()
        {
            //nothing to close
        }

        public void Dispose()
        {
            // nothing to dispose
        }

        public override bool Equals(object obj)
        {
            FileStoreConnection connection = obj as FileStoreConnection;
            return connection != null &&
                   ConnectionString == connection.ConnectionString;
        }

        public IList<T> GetAll<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int id) where T : class, IIdentifier
        {
            XmlSerializer<T> xmlSerializer = new XmlSerializer<T>();
            T syncSettings = xmlSerializer.Load(GetFilename(typeof(T).FullName));
            return syncSettings;
        }

        public override int GetHashCode()
        {
            return 1136749921 + ConnectionString.GetHashCode();
        }

        public void Open()
        {
           //nothing to open
        }

        private string GetFilename(string typeDesc)
        {
            DbConnectionStringBuilder.TryGetValue("path", out object path);
            return path + "\\" + typeDesc + ".xml";
        }

        public void Store<T>(T obj) where T : class
        {
            XmlSerializer<T> xmlSerializer = new XmlSerializer<T>();
            xmlSerializer.Save(GetFilename(typeof(T).FullName), obj);
        }
    }
}
