namespace LiveDirectorySyncEngineLogic.SyncActionModel
{
    public class SyncFileInfo
    {
        public string FullPath { get; }

        public SyncFileInfo(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
