using System;
using System.ServiceProcess;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.Log;
using LiveDirectorySyncEngineLogic.Settings;

namespace LiveDirectorySyncEngine
{
    public partial class SyncService : ServiceBase
    {

        private SyncWorker worker;

        public SyncService()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Info("SyncService started");
            //TODO implement async ISyncAction implementation with off line handling.
            ISyncSettingsRepository syncSettingsRepository = LiveDirectorySyncEngineLogic.Container.GetSyncSettingsRepository();
            SyncSettings settings = syncSettingsRepository.Load();
            worker = new SyncWorker(settings, LiveDirectorySyncEngineLogic.Container.GetRealtimeNoneCacheSyncActionHandler(settings), LiveDirectorySyncEngineLogic.Container.GetFileSystem());
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
