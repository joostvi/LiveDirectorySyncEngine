using System.ServiceProcess;
using LiveDirectorySyncEngineLogic;
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
            Log.Info("SyncService started");
            //TODO implement async ISyncAction implementation with off line handling.
            ISyncSettingsRepository syncSettingsRepository = LiveDirectorySyncEngineLogic.Container.GetSyncSettingsRepository();
            SyncSettings settings = syncSettingsRepository.Load();
            worker = new SyncWorker(settings, LiveDirectorySyncEngineLogic.Container.GetRealtimeNoneCacheSyncActionHandler(settings));
            worker.Start();
        }

        protected override void OnStop()
        {
            worker.Stop();
            Log.Info("SyncService stopped");
        }        
    }
}
