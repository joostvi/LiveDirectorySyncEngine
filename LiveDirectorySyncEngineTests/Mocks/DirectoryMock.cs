﻿using LiveDirectorySyncEngineLogic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDirectorySyncEngineTests.Mocks
{
    public class DirectoryMock : IDirectory
    {
        public IList<string> Directories = new List<string>();

        public void Create(string path)
        {
            Directories.Add(path);
        }

        public void Delete(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public string[] GetDirectories(string path)
        {
            //todo handle / or no / in the right way as this might return wrong paths.
            //Or just ensure tests are not using paths which start with simular name.
            return Directories.Where(a => a.StartsWith(path)).ToArray();
        }

        public string[] GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public void Move(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }
    }
}