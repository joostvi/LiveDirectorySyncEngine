using LiveDirectorySyncEngineLogic.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveDirectorySyncEngineLogic.Settings
{
    public class SettingsValidator
    {
        private readonly IDirectory _directory;

        public SettingsValidator(IDirectory directory)
        {
            _directory = directory;
        }

        public void IsValid(SyncSettings settings)
        {
            if (!_directory.Exists(settings.SourcePath))
            {
                throw new InvalidInputException($"Source path ({settings.SourcePath}) does not exist!");
            }
            //TODO reconsider this when implementing async.
            if (!_directory.Exists(settings.TargetPath))
            {
                throw new InvalidInputException($"Target path ({settings.TargetPath}) does not exist!");
            }
        }
    }
}
