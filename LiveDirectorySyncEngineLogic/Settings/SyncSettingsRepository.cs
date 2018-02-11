using LiveDirectorySyncEngineLogic.Generic;
using System;

namespace LiveDirectorySyncEngineLogic.Settings
{
    public class SyncSettingsRepository : ISyncSettingsRepository
    {

        private readonly string dir = AppDomain.CurrentDomain.BaseDirectory;

        public void Save(SyncSettings syncSetting)
        {
            string filename = GetFileName();
            XmlSerializer<SyncSettings> xmlSerializer = new XmlSerializer<SyncSettings>();
            xmlSerializer.Save(filename, syncSetting);
        }

        private string GetFileName()
        {
            return dir + @"SyncSettings.xml";
        }

        public SyncSettings Load()
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
