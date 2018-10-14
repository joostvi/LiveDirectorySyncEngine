using GenericClassLibrary.FileSystem;

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

            if (settings.LogPath?.Length > 0 && !_directory.Exists(settings.LogPath))
            {
                throw new InvalidInputException($"LogPath path ({settings.LogPath}) does not exist!");
            }
        }
    }
}
