using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using LiveDirectorySyncEngineLogic.Generic.DataAccess.FileSystem;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using System;

namespace LiveDirectorySyncEngineLogic
{
    public static class Container
    {
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

        public static IDBConnection GetDBConnection()
        {
            //File store implementations
            string dir = "path=" + AppDomain.CurrentDomain.BaseDirectory;
            return new FileStoreConnection(dir);
        }
    }
}
