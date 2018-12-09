using LiveDirectorySyncEngineLogic.Generic.Log;
using Xunit;
using Moq;
using System;

namespace LiveDirectorySyncEngineTests.UnitTests.Generic
{
    [Collection("LoggingTests")]
    public class LoggingTests
    {
        [Fact]
        public void Logger_Error_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Error;

            //act
            string logText = "Some error info to log";
            Logger.Error(logText);

            //verify
            logger.Verify(a => a.Error(logText));
        }

        [Fact]
        public void Logger_Info_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Logger.Info(logText);

            //verify
            logger.Verify(a => a.Info(logText));
        }

        [Fact]
        public void Logger_Debug_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Debug;

            //act
            string logText = "Some error info to log";
            Logger.Debug(logText);

            //verify
            logger.Verify(a => a.Debug(logText));
        }

        [Fact]
        public void Logger_Warning_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Warning;

            //act
            string logText = "Some warning info to log";
            Logger.Warning(logText);

            //verify
            logger.Verify(a => a.Warning(logText));
        }

        [Fact]
        public void Logger_MultipleLoggers_Test()
        {
            //setup
            Mock<ILogger> logger1 = new Mock<ILogger>();
            Logger.AddLogger(logger1.Object);
            Mock<ILogger> logger2 = new Mock<ILogger>();
            Logger.AddLogger(logger2.Object);
            Logger.Level = EnumLogLevel.Debug;

            //act
            string logText = "Some error info to log";
            Logger.Debug(logText);

            //verify
            logger1.Verify(a => a.Debug(logText));
            logger2.Verify(a => a.Debug(logText));
        }

        [Fact]
        public void Logger_LevelHigherThenRequested_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Logger.Debug(logText);

            //verify
            logger.Verify(a => a.Debug(logText), Times.Never);
        }

        [Fact]
        public void Logger_LevelLowerThenRequested_Test()
        {
            //setup
            Mock<ILogger> logger = new Mock<ILogger>();
            Logger.AddLogger(logger.Object);
            Logger.Level = EnumLogLevel.Info;

            //act
            string logText = "Some error info to log";
            Logger.Error(logText);

            //verify
            logger.Verify(a => a.Error(logText));
        }

        [Fact]
        public void LogLevelList_ContainsAllItems_Test()
        {
            LogLevelList logLevels = new LogLevelList();
            Array valuesAsArray = Enum.GetValues(typeof(EnumLogLevel));

            Assert.Equal(valuesAsArray.Length, logLevels.Count);
        }
    }
}
