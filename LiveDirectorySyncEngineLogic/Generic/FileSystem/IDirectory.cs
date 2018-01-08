namespace LiveDirectorySyncEngineLogic.Generic
{
    public interface IDirectory
    {
        void Create(string path);
        void Delete(string path, bool recursive);
        bool Exists(string path);
        void Move(string sourceDirName, string destDirName);

        string[] GetDirectories(string path);
        string[] GetFiles(string path);
    }
}