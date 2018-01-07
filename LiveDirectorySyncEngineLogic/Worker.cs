using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineLogic.Settings;
using System;
using System.IO;
using LiveDirectorySyncEngineLogic.Generic;

namespace LiveDirectorySyncEngineLogic
{
    /// <summary>
    /// This is the class doing all the actions to keep files in sync realtime.
    /// This is in a seperate class to make it usable in the service and also in the sync logic application 
    /// which will be used for testing + also for setting up the settings.
    /// </summary>
    public class RealtimeSyncWorker
    {
        private SyncSettings _Settings;
        private FileSystemWatcher _watcher;
        private ISyncAction _syncAction;
        private ISyncSettingsRepository _syncSettingsRepository;
        private IFileSystem _FileSystem;

        public RealtimeSyncWorker()
        {
            _syncSettingsRepository = Container.GetSyncSettingsRepository();
            _Settings = _syncSettingsRepository.Load();
            _syncAction = Container.GetRealtimeNoneCacheSyncActionHandler(_Settings);
            _FileSystem = Container.GetFileSystem();
        }

        public void Start()
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Worker already started? Please stop first.");
            }            
            Watch();
        }

        public void Stop()
        {
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
            _syncAction.Rename(command);
        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncAction.Delete(new SyncDeleteActionCommand() { SourceFile = syncFileInfo });
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            //not interested in update of folders as we check the folder content.
            if (_FileSystem.IsDirectory(e.FullPath)) return;
            _syncAction.Update(new SyncUpdateActionCommand() { SourceFile = syncFileInfo });
        }

        public void OnCreate(object source, FileSystemEventArgs e)
        {
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncAction.Create(new SyncCreateActionCommand() { SourceFile = syncFileInfo });
        }
    }
}
