using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineLogic.Settings;
using System;
using System.IO;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.Log;

namespace LiveDirectorySyncEngineLogic
{
    /// <summary>
    /// This is the class doing all the actions to keep files in sync realtime.
    /// This is in a seperate class to make it usable in the service and also in the sync logic application 
    /// which will be used for testing + also for setting up the settings.
    /// </summary>
    public class SyncWorker
    {
        private SyncSettings _Settings;
        private FileSystemWatcher _watcher;
        private ISyncAction _syncAction;
        private IFileSystem _FileSystem;

        public SyncWorker(SyncSettings settings, ISyncAction syncAction)
        {
            _Settings = settings;
            _syncAction = syncAction;
            _FileSystem = Container.GetFileSystem();
        }

        public void Start()
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Worker already started? Please stop first.");
            }
            Log.Info("RealtimeSyncWorker started.");
            Watch();
        }

        public void Stop()
        {
            Log.Info("RealtimeSyncWorker stopped.");
            _watcher.Dispose();
            _watcher = null;
        }

        private void Watch()
        {
            _watcher = new FileSystemWatcher();
            _watcher.IncludeSubdirectories = true;
            _watcher.Path = _Settings.SourcePath;
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*.*";
            _watcher.Created += new FileSystemEventHandler(OnCreate);
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
            _watcher.EnableRaisingEvents = true;     
            
        }

        private string AddTargetPath(string filename)
        {
            return _Settings.TargetPath + "\\" + filename;
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            SyncRenameActionCommand command = new SyncRenameActionCommand();
            command.OldFileName = e.OldName;
            command.NewFileName = e.Name;
            Log.Info($"RealtimeSyncWorker rename of {e.OldName} to {e.Name}.");
            _syncAction.Rename(command);
        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Log.Info($"RealtimeSyncWorker delete of {e.FullPath}.");
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncAction.Delete(new SyncDeleteActionCommand() { SourceFile = syncFileInfo });
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Log.Info($"RealtimeSyncWorker change of {e.FullPath}.");
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            //not interested in update of folders as we check the folder content.
            if (_FileSystem.IsDirectory(e.FullPath)) return;
            _syncAction.Update(new SyncUpdateActionCommand() { SourceFile = syncFileInfo });
        }

        public void OnCreate(object source, FileSystemEventArgs e)
        {
            Log.Info($"RealtimeSyncWorker creation of {e.FullPath}.");
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncAction.Create(new SyncCreateActionCommand() { SourceFile = syncFileInfo });
        }
    }
}
