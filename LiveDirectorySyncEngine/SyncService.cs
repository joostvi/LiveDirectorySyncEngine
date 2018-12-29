﻿using System;
using System.ServiceProcess;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using GenericClassLibrary.Logging;
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
            using (IDBConnection connection = LiveDirectorySyncEngineLogic.Container.GetDBConnection())
            {
                ISyncSettingsRepository syncSettingsRepository = LiveDirectorySyncEngineLogic.Container.GetSyncSettingsRepository(connection);
                SyncSettings settings = syncSettingsRepository.Get(1);
                worker = new SyncWorker(settings, LiveDirectorySyncEngineLogic.Container.GetRealtimeNoneCacheSyncActionHandler(settings), LiveDirectorySyncEngineLogic.Container.GetFileSystem());
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
