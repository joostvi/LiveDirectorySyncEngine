using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using System;

namespace LiveDirectorySyncEngineLogic.Settings
{
    public class SyncSettingsRepository : BaseRepository<SyncSettings>, ISyncSettingsRepository
    {
        private readonly string dir = AppDomain.CurrentDomain.BaseDirectory;

        public SyncSettingsRepository(IDBConnection odb) : base(odb)
        {
        }

        public void Save(SyncSettings syncSetting)
        {
            
            string filename = GetFileName();
            XmlSerializer<SyncSettings> xmlSerializer = new XmlSerializer<SyncSettings>();
            xmlSerializer.Save(filename, syncSetting);

            Store(syncSetting);
        }

        private string GetFileName()
        {
            return dir + @"SyncSettings.xml";
        }

        public SyncSettings Get()
        {
            string filename = GetFileName();
            XmlSerializer<SyncSettings> xmlSerializer = new XmlSerializer<SyncSettings>();
            SyncSettings syncSettings = xmlSerializer.Load(filename);
            if (syncSettings == null)
            {
                syncSettings = new SyncSettings();
            }
            return syncSettings;
        }
    }
}
