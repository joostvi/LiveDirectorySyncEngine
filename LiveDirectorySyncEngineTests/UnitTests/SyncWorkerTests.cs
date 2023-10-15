using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using GenericClassLibraryTests.Mocks;
using GenericClassLibrary.Logging;
using GenericClassLibrary.Validation;
using NSubstitute;
using Xunit;
using System.Threading;
using GenericNSubstituteTestHelpers;

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
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper
            {
                FileExists = true,
                DirectoryExists = false
            };
            mockHelper.Setup();
            var syncActionHandler = Substitute.For<ISyncActionHandler>();
            syncActionHandler.When( a => a.CanStart()).Do( a => throw new InvalidInputException());

            SyncSettings syncSettings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, EnumLogLevel.Info, _DefaultLogPath);
            SyncWorker syncWorker = new SyncWorker(syncSettings, syncActionHandler, mockHelper.IFileSystemMock, new CancellationToken());
            Assert.Throws<InvalidInputException>( () => syncWorker.Start());
            syncActionHandler.Received().CanStart();
            mockHelper.IDirectoryMock.VerifyNoCalls();
        }
    }
}
