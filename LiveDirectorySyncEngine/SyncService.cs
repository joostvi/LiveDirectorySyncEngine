using System;
using System.ServiceProcess;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using GenericClassLibrary.Logging;
using LiveDirectorySyncEngineLogic.Settings;
using System.Threading;

namespace LiveDirectorySyncEngine
{
    public partial class SyncService : ServiceBase
    {

        private readonly CancellationTokenSource _source;
        private SyncWorker worker;

        public SyncService()
        {
            _source = new CancellationTokenSource();
        }

        protected override void OnStart(string[] args)
        {
            
            CancellationToken token = _source.Token;

            Logger.Info("SyncService started");
            //TODO implement async ISyncAction implementation with off line handling.
            using (IDBConnection connection = LiveDirectorySyncEngineLogic.Container.GetDBConnection())
            {
                ISyncSettingsRepository syncSettingsRepository = LiveDirectorySyncEngineLogic.Container.GetSyncSettingsRepository(connection);
                SyncSettings settings = syncSettingsRepository.Get(1);
                worker = new SyncWorker(settings, LiveDirectorySyncEngineLogic.Container.GetRealtimeNoneCacheSyncActionHandler(settings), LiveDirectorySyncEngineLogic.Container.GetFileSystem(), token);
            }
            try
            {
                worker.Start();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to start the sync service", ex);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            try
            {
                _source.Cancel();
                worker.Stop();
                Logger.Info("SyncService stopped");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while stopping the service", ex);
            }

        }
    }
}
