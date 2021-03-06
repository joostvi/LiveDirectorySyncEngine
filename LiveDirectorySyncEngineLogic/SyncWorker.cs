﻿using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineLogic.Settings;
using System;
using System.IO;
using GenericClassLibrary.FileSystem;
using GenericClassLibrary.Logging;
using System.Threading;

namespace LiveDirectorySyncEngineLogic
{
    /// <summary>
    /// This is the class doing all the actions to keep files in sync realtime.
    /// This is in a seperate class to make it usable in the service and also in the sync logic application 
    /// which will be used for testing + also for setting up the settings.
    /// </summary>
    public class SyncWorker
    {
        private readonly SyncSettings _Settings;
        private FileSystemWatcher _watcher;
        private readonly ISyncActionHandler _syncActionHandlere;
        private readonly IFileSystem _FileSystem;
        private readonly CancellationToken _cancellationToken;

        public SyncWorker(SyncSettings settings, ISyncActionHandler syncActionHandlere, IFileSystem fileSystem, CancellationToken cancellationToken)
        {
            _Settings = settings;
            _syncActionHandlere = syncActionHandlere;
            _FileSystem = fileSystem;
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            CanStart();
            Logger.Info("SyncWorker started.");
            Watch();
        }

        public void Stop()
        {
            Logger.Info("SyncWorker stopped.");
            HandleCancellation();
            _watcher.Dispose();
            _watcher = null;
        }

        private void CanStart()
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Worker already started? Please stop first.");
            }
            _syncActionHandlere.CanStart();
        }

        private void Watch()
        {
            _watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                Path = _Settings.SourcePath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };
            _watcher.Created += new FileSystemEventHandler(OnCreate);
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
            _watcher.Error += new ErrorEventHandler(OnError);
            _watcher.EnableRaisingEvents = true;

        }

        private void HandleCancellation()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _watcher.EnableRaisingEvents = false;
            }
        }

        public void OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            Logger.Error($"FileSystem watchers got an Error of type: {ex.GetType()}, Message: {ex.Message}.");
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            SyncRenameActionCommand command = new SyncRenameActionCommand
            {
                OldFileName = e.OldName,
                NewFileName = e.Name
            };
            Logger.Info($"SyncWorker rename of {e.OldName} to {e.Name}.");
            _syncActionHandlere.Rename(command);
            HandleCancellation();
        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Logger.Info($"SyncWorker delete of {e.FullPath}.");
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncActionHandlere.Delete(new SyncDeleteActionCommand() { SourceFile = syncFileInfo });
            HandleCancellation();
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.Info($"SyncWorker change of {e.FullPath}.");
            //It appears we get changed while target also is deleted.
            bool found = false;
            int attempts = 0;
            while (!found && attempts < 10)
            {
                found = ExistsFileOrFolder(e.FullPath);
                if (!found)
                {
                    Thread.Sleep(500);
                    attempts += 1;
                }
            }
            if (!found)
            {
                Logger.Info($"Got file change message for none existing file or folder {e.FullPath}");
                return;
            }
            //not interested in update of folders as we check the folder content.

            if (_FileSystem.IsDirectory(e.FullPath)) return;
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncActionHandlere.Update(new SyncUpdateActionCommand() { SourceFile = syncFileInfo });
            HandleCancellation();
        }

        private bool ExistsFileOrFolder(string fileOrFolder)
        {
            return _FileSystem.File.Exists(fileOrFolder) || _FileSystem.Directory.Exists(fileOrFolder);
        }

        public void OnCreate(object source, FileSystemEventArgs e)
        {
            Logger.Info($"SyncWorker creation of {e.FullPath}.");
            SyncFileInfo syncFileInfo = new SyncFileInfo(e.FullPath);
            _syncActionHandlere.Create(new SyncCreateActionCommand() { SourceFile = syncFileInfo });
            HandleCancellation();
        }
    }
}
