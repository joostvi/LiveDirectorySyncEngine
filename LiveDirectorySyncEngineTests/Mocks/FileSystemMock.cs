using LiveDirectorySyncEngineLogic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDirectorySyncEngineTests.Mocks
{
    public class FileSystemMock : IFileSystem
    {
        public IDirectory Directory { get; }

        public IFile File { get; }

        public FileSystemMock()
        {
            DirectoryMock directory = new DirectoryMock();
            Directory = directory;
            File = new FileMock(directory);
        }

        public bool IsDirectory(string fullPath)
        {
            //in tests we assume paths do not contain dots.
            return !fullPath.Contains('.');
        }
    }
}
