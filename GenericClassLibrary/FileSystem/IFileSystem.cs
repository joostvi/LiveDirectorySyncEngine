namespace GenericClassLibrary.FileSystem
{
    public interface IFileSystem
    {
        IDirectory Directory { get; }
        IFile File { get;  }

        bool IsDirectory(string fullPath);
    }
}