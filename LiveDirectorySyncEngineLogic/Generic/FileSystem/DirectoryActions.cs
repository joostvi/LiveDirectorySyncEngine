using System.IO;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public class DirectoryActions : IDirectory
    {
        public void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        public void Delete(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public void Create(string Path)
        {
            Directory.CreateDirectory(Path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        
    }
}
