using System;
using System.IO;

namespace LiveDirectorySyncEngineLogic.Generic.Log
{
    public class FileLogger : ILogger
    {
        private readonly string _baseFileName;
        private readonly string _path;

        public FileLogger(string path, string baseFileName)
        {
            _path = path;
            _baseFileName = baseFileName;
        }

        private string GetFileName()
        {
            return _path + "\\" + _baseFileName + "_" + DateTime.Now.ToString("yyyymmdd") + ".log";
        }

        private void DoLog(string value)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(GetFileName(), true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " " + value);
                }
            }
            catch (Exception)
            {
                //Do nothing. Logging should not prevent normal process flow.
            }
        }

        public void Debug(string value)
        {
            DoLog("DEBUG " + value);
        }

        public void Error(string value)
        {
            DoLog("ERROR " + value);
        }

        public void Info(string value)
        {
            DoLog("INFO " + value);
        }
    }
}
