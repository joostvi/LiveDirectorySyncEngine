using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using GenericClassLibraryTests.Mocks;
using GenericClassLibrary.Logging;
using GenericClassLibrary.Validation;
using Moq;
using Xunit;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [Collection("SyncWorkerTests")]
    public class SyncWorkerTests
    {

        private const string _DefaultSourcePath = "C:\\someFolder\\";
        private const string _DefaultTargetPath = "D:\\Anotherfolder\\";
        private const string _DefaultLogPath = "D:\\Logfolder\\";

        [Fact]
        public void CanStart_IsCalledAndExceptionIsThrow()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();
            Mock<ISyncActionHandler> syncActionHandler = new Mock<ISyncActionHandler>();
            syncActionHandler.Setup(a => a.CanStart()).Throws<InvalidInputException>();

            SyncSettings syncSettings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, EnumLogLevel.Info, _DefaultLogPath);
            SyncWorker syncWorker = new SyncWorker(syncSettings, syncActionHandler.Object, mockHelper.IFileSystemMock.Object);
            Assert.Throws<InvalidInputException>( () => syncWorker.Start());
            syncActionHandler.Verify(a => a.CanStart());
            mockHelper.IDirectoryMock.VerifyNoOtherCalls();
        }
    }
}
