using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public static partial class Log
    {
        private static object _lock = new object();
        private static List<ILogger> _loggers = new List<ILogger>();

        public static LogLevel Level { get; set; }

        public static void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        private static void DoLog(LogLevel level, string value)
        {
            if (level > Level)
            {
                return;
            }

            foreach (ILogger logger in _loggers)
            {
                switch (level)
                {
                    case LogLevel.Error:
                        logger.Error(value);
                        break;
                    case LogLevel.Info:
                        logger.Info(value);
                        break;
                    case LogLevel.Debug:
                        logger.Debug(value);
                        break;
                }
            }
        }

        public static void Error(string value)
        {
            DoLog(LogLevel.Error, value);
        }

        public static void Info(string value)
        {
            DoLog(LogLevel.Info, value);
        }

        public static void Debug(string value)
        {
            DoLog(LogLevel.Debug, value);
        }
    }
}
