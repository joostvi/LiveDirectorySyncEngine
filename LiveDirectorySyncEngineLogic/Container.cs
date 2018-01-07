using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;

namespace LiveDirectorySyncEngineLogic
{
    public static class Container
    {
        public static ISyncSettingsRepository GetSyncSettingsRepository()
        {
            return new SyncSettingsRepository();
        }

        public static ISyncAction GetRealtimeNoneCacheSyncActionHandler(SyncSettings settings)
        {
            return new RealtimeNoneCacheSyncActionHandler(settings, GetFileSystem());
        }

        public static IFileSystem GetFileSystem()
        {
            return new FileSystem();
        }
    }
}
