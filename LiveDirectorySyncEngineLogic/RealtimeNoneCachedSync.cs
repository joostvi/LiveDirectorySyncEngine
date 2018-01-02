using System.IO;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineLogic.Settings;

namespace LiveDirectorySyncEngineLogic
{
    public class GetRealtimeNoneCacheSyncActionHandler : ISyncAction
    {
        private SyncSettings _Settings;

        public GetRealtimeNoneCacheSyncActionHandler(SyncSettings settings)
        {
            _Settings = settings;
        }

        private string AddTargetPath(string filename)
        {
            return _Settings.TargetPath + "\\" + filename;
        }

        private string AddSourcePath(string filename)
        {
            return _Settings.SourcePath + "\\" + filename;
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
            if (FileHelpers.IsDirectory(oldName))
            {
                Directory.Move(oldName, newName);
                return;
            }
            File.Move(oldName, newName);
        }

        public void Delete(SyncDeleteActionCommand command)
        {
            string aFile = command.SourceFile.FullPath;
            string aTarget = aFile.Replace(_Settings.SourcePath, _Settings.TargetPath);
            if (!ExistsFileOrFolder(aTarget)) return;

            if (FileHelpers.IsDirectory(aTarget))
            {
                Directory.Delete(aTarget, true);
                return;
            }
            File.Delete(aTarget);
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
            File.Copy(aFile, aTarget, true);
        }

        public void Create(SyncCreateActionCommand command)
        {
            //Copies file to another directory.
            string aSource = command.SourceFile.FullPath;

            string aTarget = aSource.Replace(_Settings.SourcePath, _Settings.TargetPath);
            Create(aSource, aTarget);
        }

        private static void Create(string aSource, string aTarget)
        {
            if (FileHelpers.IsDirectory(aSource))
            {
                Directory.CreateDirectory(aTarget);
                return;
            }
            File.Copy(aSource, aTarget, true);
        }

        private static bool ExistsFileOrFolder(string fileOrFolder)
        {
            return File.Exists(fileOrFolder) || Directory.Exists(fileOrFolder);
        }
    }
}
