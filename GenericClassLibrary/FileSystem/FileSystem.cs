using System.IO;

namespace GenericClassLibrary.FileSystem
{
    public class FileSystem : IFileSystem
    {
        private static readonly IDirectory _Directory = new DirectoryActions();
        private static readonly IFile _File = new FileActions();

        public IDirectory Directory => _Directory;

        public IFile File => _File;

        public bool IsDirectory(string fullPath)
        {
            return System.IO.File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);
        }
    }
}
