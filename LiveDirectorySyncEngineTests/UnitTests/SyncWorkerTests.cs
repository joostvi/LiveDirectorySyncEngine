using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [TestClass]
    public class SyncWorkerTests
    {

        private const string _DefaultSourcePath = "C:\\someFolder\\";
        private const string _DefaultTargetPath = "D:\\Anotherfolder\\";

        [TestMethod]
        public void CanStart_IsCalledAndExceptionIsThrow()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();
            Mock<ISyncActionHandler> syncActionHandler = new Mock<ISyncActionHandler>();
            syncActionHandler.Setup(a => a.CanStart()).Throws<InvalidInputException>();

            SyncSettings syncSettings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, LiveDirectorySyncEngineLogic.Generic.Log.EnumLogLevel.Info);
            SyncWorker syncWorker = new SyncWorker(syncSettings, syncActionHandler.Object, mockHelper.IFileSystemMock.Object);
            Assert.ThrowsException<InvalidInputException>( () => syncWorker.Start());
            syncActionHandler.Verify(a => a.CanStart());
            mockHelper.IDirectoryMock.VerifyNoOtherCalls();
        }
    }
}
