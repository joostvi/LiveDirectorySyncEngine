using System;
using System.IO;

namespace LiveDirectorySyncEngineLogic
{
    /// <summary>
    /// This is the class doing all the actions to keep files in sync realtime.
    /// This is in a seperate class to make it usable in the service and also in the sync logic application 
    /// which will be used for testing + also for setting up the settings.
    /// </summary>
    public class RealtimeSyncWorker
    {
        private Settings _Settings;
        private FileSystemWatcher _watcher;

        public void Start()
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Worker already started? Please stop first.");
            }
            _Settings = new Settings("C:\\tmp\\TestSource", "C:\\tmp\\TestTarget");
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
            _watcher.Path = _Settings.SourcePath;
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.Filter = "*.*";
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
            _watcher.EnableRaisingEvents = true;
            
        }

        private string AddTargetPath(string filename)
        {
            return _Settings.TargetPath + "\\" + filename;
        }
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string oldName = e.OldName;
            string newName = e.Name;
            File.Move(AddTargetPath(oldName), AddTargetPath(newName));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //Copies file to another directory.
            string aFile = e.FullPath;
            string aTarget = aFile.Replace(_Settings.SourcePath, _Settings.TargetPath);
            File.Copy(aFile, aTarget, true);
        }
    }
}
