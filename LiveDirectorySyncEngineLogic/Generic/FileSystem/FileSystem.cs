using System.IO;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public class FileSystem : IFileSystem
    {
        private static IDirectory _Directory = new DirectoryActions();
        private static IFile _File = new FileActions();

        public IDirectory Directory => _Directory;

        public IFile File => _File;

        public bool IsDirectory(string fullPath)
        {
            return System.IO.File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);
        }
    }
}
