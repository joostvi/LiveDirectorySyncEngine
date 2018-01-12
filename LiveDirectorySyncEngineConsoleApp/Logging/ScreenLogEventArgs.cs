using LiveDirectorySyncEngineLogic.Generic.Log;

namespace LiveDirectorySyncEngineConsoleApp.Logging
{
    public class ScreenLogEventArgs
    {
        public EnumLogLevel Level { get; }
        public string Value { get; }

        public ScreenLogEventArgs(EnumLogLevel level, string value)
        {
            Level = level;
            Value = value;
        }
    }
}
