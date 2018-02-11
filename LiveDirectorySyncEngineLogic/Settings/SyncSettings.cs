using LiveDirectorySyncEngineLogic.Generic.Log;
using System;

namespace LiveDirectorySyncEngineLogic.Settings
{
    [Serializable()]

    public class SyncSettings
    {
        public string SourcePath { get; set; }

        public string TargetPath { get; set; }
        public EnumLogLevel LogLevel { get; set; }

        public SyncSettings(string sourcePath, string targetPath, EnumLogLevel logLevel )
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            LogLevel = logLevel;
        }
        
        public SyncSettings()
        {
            SourcePath = @"C:\tmp\TestSource";
            TargetPath = @"C:\tmp\TestTarget";
        }
    }
}
