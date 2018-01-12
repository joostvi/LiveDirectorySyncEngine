using System;
using System.Collections.Generic;
using System.Text;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public interface ILogger
    {
        void Error(string value);
        void Info(string value);
        void Debug(string value);
    }
}
