using System;
using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public class LogLevelList : List<LogLevel>
    {
        public LogLevelList()
        {
            Array valuesAsArray = Enum.GetValues(typeof(EnumLogLevel));
            foreach (int value in valuesAsArray)
            {
                this.Add(new LogLevel((EnumLogLevel)value));
            }
        }
    }
}
