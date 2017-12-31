using System;

namespace LiveDirectorySyncEngineLogic
{
    public class Settings
    {
        public string SourcePath { get; }

        public string TargetPath { get; }

        public Settings(string sourcePath, string targetPath)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
        }
    }
}
