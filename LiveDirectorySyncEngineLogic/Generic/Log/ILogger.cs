
namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public interface ILogger
    {
        void Error(string value);
        void Info(string value);
        void Debug(string value);
        void Warning(string value);
    }
}
