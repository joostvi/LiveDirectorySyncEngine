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
        public string LogPath { get; set; }

        public SyncSettings(string sourcePath, string targetPath, EnumLogLevel logLevel, string logPath )
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            LogLevel = logLevel;
            LogPath = logPath;
        }
        
        public SyncSettings()
        {
            SourcePath = @"C:\tmp\TestSource";
            TargetPath = @"C:\tmp\TestTarget";
            LogLevel = EnumLogLevel.Info;
            LogPath = @"c:\tmp";
        }
    }
}
