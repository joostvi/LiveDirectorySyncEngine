using LiveDirectorySyncEngineLogic.Generic.Log;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public class LogLevel
    {
        public EnumLogLevel Level { get; }
        public string Description => Level.ToString();

        public LogLevel(EnumLogLevel level)
        {
            Level = level;
        }
    }
}
