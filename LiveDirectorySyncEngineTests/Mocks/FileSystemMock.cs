﻿using GenericClassLibrary.FileSystem;
using System.Linq;

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
