using System.IO;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public class FileActions : IFile
    {
        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

    }
}
