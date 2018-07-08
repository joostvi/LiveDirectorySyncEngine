using LiveDirectorySyncEngineLogic.Generic.Log;
using System;

namespace LiveDirectorySyncEngineConsoleApp.Logging
{
    public class ScreenLogger : ILogger
    {

        public event EventHandler<ScreenLogEventArgs> _LogEventHandler;

        public ScreenLogger(EventHandler<ScreenLogEventArgs> logEventHandler)
        {
            _LogEventHandler = logEventHandler;
        }

        private void DoLog(EnumLogLevel level, string value)
        {
            _LogEventHandler?.Invoke(this, new ScreenLogEventArgs(level, value));
        }

        public void Debug(string value)
        {
            DoLog(EnumLogLevel.Debug, value);
        }

        public void Error(string value)
        {
            DoLog(EnumLogLevel.Error, value);
        }

        public void Info(string value)
        {
            DoLog(EnumLogLevel.Info, value);
        }

        public void Warning(string value)
        {
            DoLog(EnumLogLevel.Warning, value);
        }
    }
}
