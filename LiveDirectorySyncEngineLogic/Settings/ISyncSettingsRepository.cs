namespace LiveDirectorySyncEngineLogic.Settings
{
    public interface ISyncSettingsRepository
    {
        SyncSettings Load();
        void Save(SyncSettings syncSetting);
    }
}