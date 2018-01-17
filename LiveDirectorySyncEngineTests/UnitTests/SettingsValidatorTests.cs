using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [TestClass]
    public class SettingsValidatorTests
    {

        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";

        private SyncSettings GetSettings()
        {
            return new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, LiveDirectorySyncEngineLogic.Generic.Log.EnumLogLevel.Info);
        }

        [TestMethod]
        public void CanStart_MissingSourcePath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();
            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock.Object);
            Assert.ThrowsException<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultSourcePath));
            mockHelper.IDirectoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void CanStart_MissingTargetPath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = true;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Setup(a => a.Exists(_DefaultTargetPath)).Returns(false);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock.Object);
            Assert.ThrowsException<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultSourcePath));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultTargetPath));
        }
    }
}
