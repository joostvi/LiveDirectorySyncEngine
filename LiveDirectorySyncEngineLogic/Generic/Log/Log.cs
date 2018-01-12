using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public static partial class Log
    {
        private static object _lock = new object();
        private static List<ILogger> _loggers = new List<ILogger>();

        public static EnumLogLevel Level { get; set; }

        public static void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        private static void DoLog(EnumLogLevel level, string value)
        {
            if (level > Level)
            {
                return;
            }

            foreach (ILogger logger in _loggers)
            {
                switch (level)
                {
                    case EnumLogLevel.Error:
                        logger.Error(value);
                        break;
                    case EnumLogLevel.Info:
                        logger.Info(value);
                        break;
                    case EnumLogLevel.Debug:
                        logger.Debug(value);
                        break;
                }
            }
        }

        public static void Error(string value)
        {
            DoLog(EnumLogLevel.Error, value);
        }

        public static void Info(string value)
        {
            DoLog(EnumLogLevel.Info, value);
        }

        public static void Debug(string value)
        {
            DoLog(EnumLogLevel.Debug, value);
        }
    }
}
