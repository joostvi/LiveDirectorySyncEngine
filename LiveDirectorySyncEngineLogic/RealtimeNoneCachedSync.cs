using System.IO;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.SyncActionModel;

namespace LiveDirectorySyncEngineLogic
{
    public class RealtimeNoneCachedSync : ISyncAction
    {
        private Settings _Settings;

        public RealtimeNoneCachedSync(Settings settings)
        {
            _Settings = settings;
        }

        private string AddTargetPath(string filename)
        {
            return _Settings.TargetPath + "\\" + filename;
        }

        public void Rename(SyncRenameActionCommand command)
        {
            string oldName = AddTargetPath(command.OldFileName);
            string newName = AddTargetPath(command.NewFileName);
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
            if (!File.Exists(aTarget) && !Directory.Exists(aTarget)) return;

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
            File.Copy(aFile, aTarget, true);
        }

        public void Create(SyncCreateActionCommand command)
        {
            //Copies file to another directory.
            string aFile = command.SourceFile.FullPath;

            string aTarget = aFile.Replace(_Settings.SourcePath, _Settings.TargetPath);
            if (command.SourceFile.IsDirectory())
            {
                Directory.CreateDirectory(aTarget);
                return;
            }
            File.Copy(aFile, aTarget, true);
        }
    }
}
