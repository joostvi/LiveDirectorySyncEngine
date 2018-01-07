namespace LiveDirectorySyncEngineLogic.Generic
{
    public interface IDirectory
    {
        void Create(string Path);
        void Delete(string path, bool recursive);
        bool Exists(string path);
        void Move(string sourceDirName, string destDirName);
    }
}