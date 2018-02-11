using LiveDirectorySyncEngineLogic.Generic.Log;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess.NDatabaseImplementation
{
    public class NDatabaseLogger : NDatabase.ILogger
    {
 
        public void Debug(string message)
        {
            Logger.Debug(message + "(NDatabase)");
        }

        public void Error(string message)
        {
            Logger.Error(message + "(NDatabase)");
        }

        public void Info(string message)
        {
            Logger.Info(message + "(NDatabase)");
        }

        public void Warning(string message)
        {
            Logger.Warning(message + "(NDatabase)");
        }
    }
}
