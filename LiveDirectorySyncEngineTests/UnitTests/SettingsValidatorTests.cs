using LiveDirectorySyncEngineLogic.Settings;
using GenericClassLibraryTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [TestClass]
    public class SettingsValidatorTests
    {

        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";
        private const string _DefaultLogPath = @"c:\logpath\";

        private SyncSettings GetSettings(string logPath = _DefaultLogPath)
        {
            return new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, LiveDirectorySyncEngineLogic.Generic.Log.EnumLogLevel.Info, logPath);
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

        [TestMethod]
        public void CanStart_NotExistingLogPath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = true;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Setup(a => a.Exists(_DefaultLogPath)).Returns(false);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock.Object);
            Assert.ThrowsException<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultSourcePath));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultTargetPath));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultLogPath));
        }

        [TestMethod]
        public void CanStart_LogPathNotSet_IsAllowed()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Setup(a => a.Exists(_DefaultSourcePath)).Returns(true);
            mockHelper.IDirectoryMock.Setup(a => a.Exists(_DefaultTargetPath)).Returns(true);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock.Object);
            settingsValidator.IsValid(GetSettings(logPath: string.Empty));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultSourcePath));
            mockHelper.IDirectoryMock.Verify(a => a.Exists(_DefaultTargetPath));
            mockHelper.IDirectoryMock.VerifyNoOtherCalls();
        }
    }
}
