using System.IO;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public static class FileHelpers
    {
        public static bool IsDirectory(string fullPath)
        {
            return File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);
        }
    }
}
