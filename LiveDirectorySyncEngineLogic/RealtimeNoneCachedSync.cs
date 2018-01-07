using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineLogic.Settings;

namespace LiveDirectorySyncEngineLogic
{
    public class RealtimeNoneCacheSyncActionHandler : ISyncAction
    {
        private SyncSettings _Settings;
        private IFileSystem _FileSystem;

        public RealtimeNoneCacheSyncActionHandler(SyncSettings settings, IFileSystem fileSystem)
        {
            _Settings = settings;
            _FileSystem = fileSystem;
        }

        private static string ConcatPathAndFile(string path, string filename)
        {
            return path.TrimEnd('\\') + "\\" + filename.TrimStart('\\');
        }

        private string AddTargetPath(string filename)
        {
            return ConcatPathAndFile( _Settings.TargetPath, filename);
        }

        private string AddSourcePath(string filename)
        {
            return ConcatPathAndFile(_Settings.SourcePath, filename);
        }

        public void Rename(SyncRenameActionCommand command)
        {
            string oldName = AddTargetPath(command.OldFileName);
            string newName = AddTargetPath(command.NewFileName);
            if (!ExistsFileOrFolder(oldName))
            {
                //oeps stuff not in sync we need to repair
                Create(AddSourcePath(command.NewFileName), newName);
                //TODO handle copy of content as well in case of directory
                return;
            }
            if (_FileSystem.IsDirectory(oldName))
            {
                _FileSystem.Directory.Move(oldName, newName);
                return;
            }
            _FileSystem.File.Move(oldName, newName);
        }

        public void Delete(SyncDeleteActionCommand command)
        {
            string aFile = command.SourceFile.FullPath;
            string aTarget = aFile.Replace(_Settings.SourcePath, _Settings.TargetPath);
            if (!ExistsFileOrFolder(aTarget)) return;

            if (_FileSystem.IsDirectory(aTarget))
            {
                _FileSystem.Directory.Delete(aTarget, true);
                return;
            }
            _FileSystem.File.Delete(aTarget);
        }

        public void Update(SyncUpdateActionCommand command)
        {
            //Copies file to another directory.
            string aFile = command.SourceFile.FullPath;
            string aTarget = aFile.Replace(_Settings.SourcePath, _Settings.TargetPath);
            if (!ExistsFileOrFolder(aTarget))
            {
                //oeps stuff not in sync we need to repair
                Create(aFile, aTarget);
                //TODO Handle update content as well in case of directory.
                return;
            }
            _FileSystem.File.Copy(aFile, aTarget, true);
        }

        public void Create(SyncCreateActionCommand command)
        {
            //Copies file to another directory.
            string aSource = command.SourceFile.FullPath;

            string aTarget = aSource.Replace(_Settings.SourcePath, _Settings.TargetPath);
            Create(aSource, aTarget);
        }

        private void Create(string aSource, string aTarget)
        {
            if (_FileSystem.IsDirectory(aSource))
            {
                _FileSystem.Directory.Create(aTarget);
                return;
            }
            _FileSystem.File.Copy(aSource, aTarget, true);
        }

        private bool ExistsFileOrFolder(string fileOrFolder)
        {
            return _FileSystem.File.Exists(fileOrFolder) || _FileSystem.Directory.Exists(fileOrFolder);
        }
    }
}
