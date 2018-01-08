using LiveDirectorySyncEngineLogic.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace LiveDirectorySyncEngineTests.Mocks
{
    public class FileMock : IFile
    {
        public IList<string> Files = new List<string>();
        private DirectoryMock _Directory;

        public FileMock(DirectoryMock directory)
        {
            _Directory = directory;
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            string path = destFileName.Substring(0, destFileName.LastIndexOf("\\"));
            if (!_Directory.Directories.Contains(path) )
            {
                throw new DirectoryNotFoundException();
            }
            Files.Add(destFileName);
        }

        public void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public void Move(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }
    }
}
