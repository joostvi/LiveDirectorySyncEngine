using System.ServiceProcess;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Settings;

namespace LiveDirectorySyncEngine
{
    public partial class SyncService : ServiceBase
    {

        private RealtimeSyncWorker worker;

        public SyncService()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            worker = new RealtimeSyncWorker();
            worker.Start();
        }

        protected override void OnStop()
        {
            worker.Stop();
        }        
    }
}
