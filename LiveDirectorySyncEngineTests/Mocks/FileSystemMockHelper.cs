using GenericClassLibrary.FileSystem;
using Moq;

namespace LiveDirectorySyncEngineTests.Mocks
{
    public class FileSystemMoqHelper
    {
        public bool FileExists { get; set; }
        public bool DirectoryExists { get; set; }
        public bool IsDirectory { get; set; }

        public Mock<IFileSystem> IFileSystemMock { get; private set; }
        public Mock<IFile> IFileMock { get; private set; }
        public Mock<IDirectory> IDirectoryMock { get; private set; }

        public void Setup()
        {
            IFileSystemMock = new Mock<IFileSystem>();
            IFileMock = new Mock<IFile>();
            IFileMock.Setup(a => a.Exists(It.IsAny<string>())).Returns(FileExists);

            IDirectoryMock = new Mock<IDirectory>();
            IDirectoryMock.Setup(a => a.Exists(It.IsAny<string>())).Returns(DirectoryExists);

            IFileSystemMock.SetupGet(b => b.File).Returns(IFileMock.Object);
            IFileSystemMock.SetupGet(b => b.Directory).Returns(IDirectoryMock.Object);
            IFileSystemMock.Setup(c => c.IsDirectory(It.IsAny<string>())).Returns(IsDirectory);
        }
    }
}
