﻿using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess.NDatabaseImplementation;
using LiveDirectorySyncEngineLogic.Generic.DataAccess.FileSystem;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using NDatabase.Api;
using System;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;

namespace LiveDirectorySyncEngineLogic
{
    public static class Container
    {
        private const bool _useNDatabase = true;

        public static ISyncSettingsRepository GetSyncSettingsRepository(IDBConnection dBConnection)
        {
            return new SyncSettingsRepository(dBConnection);
        }

        public static ISyncActionHandler GetRealtimeNoneCacheSyncActionHandler(SyncSettings settings)
        {
            return new RealtimeNoneCachedSyncActionHandler(settings, GetFileSystem());
        }

        public static IFileSystem GetFileSystem()
        {
            return new FileSystem();
        }

        public static void Setup()
        {
            if (_useNDatabase && !OdbConfiguration.IsLoggingEnabled())
            {
                var customLogger = new NDatabaseLogger();
                OdbConfiguration.RegisterLogger(customLogger);
                OdbConfiguration.EnableLogging();
            }
        }


        public static IDBConnection GetDBConnection()
        {
           
            //TODO: file location as setting.

            //ndatabase implementation
            if (_useNDatabase)
            {
                string database = AppDomain.CurrentDomain.BaseDirectory + @"queue.db";
                return new NDatabaseConnection(database);
            }

            //File store implementations
            string dir = "path=" + AppDomain.CurrentDomain.BaseDirectory;
            return new FileStoreConnection(dir);
        }

    }
}
