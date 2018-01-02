using System;

namespace LiveDirectorySyncEngineLogic.Settings
{
    [Serializable()]

    public class SyncSettings
    {
        public string SourcePath { get; set; }

        public string TargetPath { get; set; }

        public SyncSettings(string sourcePath, string targetPath)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
        }
        
        public SyncSettings()
        {

        }
    }
}
