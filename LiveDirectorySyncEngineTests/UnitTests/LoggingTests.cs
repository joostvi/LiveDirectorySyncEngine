using LiveDirectorySyncEngineLogic.Generic.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void Log_Error_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Log.AddLogger(logger.Object);
            Log.Level = EnumLogLevel.Error;

            //act
            string logText = "Some error info to log";
            Log.Error(logText);

            //verify
            logger.Verify(a => a.Error(logText));
        }

        [TestMethod]
        public void Log_Info_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Log.AddLogger(logger.Object);
            Log.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Log.Info(logText);

            //verify
            logger.Verify(a => a.Info(logText));
        }

        [TestMethod]
        public void Log_Debug_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Log.AddLogger(logger.Object);
            Log.Level = EnumLogLevel.Debug;

            //act
            string logText = "Some error info to log";
            Log.Debug(logText);

            //verify
            logger.Verify(a => a.Debug(logText));
        }

        [TestMethod]
        public void Log_MultipleLoggers_Test()
        {
            //setup
            Mock<ILogger> logger1 = new Mock<ILogger>();
            Log.AddLogger(logger1.Object);
            Mock<ILogger> logger2 = new Mock<ILogger>();
            Log.AddLogger(logger2.Object);
            Log.Level = EnumLogLevel.Debug;

            //act
            string logText = "Some error info to log";
            Log.Debug(logText);

            //verify
            logger1.Verify(a => a.Debug(logText));
            logger2.Verify(a => a.Debug(logText));
        }

        [TestMethod]
        public void Log_LevelHigherThenRequested_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Log.AddLogger(logger.Object);
            Log.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Log.Debug(logText);

            //verify
            logger.Verify(a => a.Debug(logText), Times.Never);
        }

        [TestMethod]
        public void Log_LevelLowerThenRequested_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Log.AddLogger(logger.Object);
            Log.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Log.Error(logText);

            //verify
            logger.Verify(a => a.Error(logText));
        }
    }
}
