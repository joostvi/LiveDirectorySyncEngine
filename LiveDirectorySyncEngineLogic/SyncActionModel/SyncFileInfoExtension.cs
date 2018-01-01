using LiveDirectorySyncEngineLogic.Generic;

namespace LiveDirectorySyncEngineLogic.SyncActionModel
{
    public static class SyncFileInfoExtensions
    {
        /// <summary>
        /// This extension checks the type by reading file from disk.
        /// That is not something I think should be part of a model.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static bool IsDirectory(this SyncFileInfo fileInfo)
        {
            return FileHelpers.IsDirectory(fileInfo.FullPath);
        }
    }
}
