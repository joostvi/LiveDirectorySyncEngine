using LiveDirectorySyncEngineLogic.Settings;
using GenericClassLibraryTests.Mocks;
using GenericClassLibrary.Validation;
using GenericClassLibrary.Logging;
using Xunit;
using NSubstitute;
using GenericNSubstituteTestHelpers;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [Collection("SettingsValidatorTests")]
    public class SettingsValidatorTests
    {

        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";
        private const string _DefaultLogPath = @"c:\logpath\";

        private static SyncSettings GetSettings(string logPath = _DefaultLogPath)
        {
            return new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, EnumLogLevel.Info, logPath);
        }

        [Fact]
        public void CanStart_MissingSourcePath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();
            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock);
            Assert.Throws<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.VerifyNoOtherCalls( new System.Collections.Generic.List<string>() {"Exists"});
        }

        [Fact]
        public void CanStart_MissingTargetPath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper
            {
                FileExists = true,
                DirectoryExists = true
            };
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Exists(_DefaultTargetPath).Returns(false);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock);
            Assert.Throws<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.Received().Exists(_DefaultTargetPath);
        }

        [Fact]
        public void CanStart_NotExistingLogPath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = true;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Exists(_DefaultLogPath).Returns(false);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock);
            Assert.Throws<InvalidInputException>(() => settingsValidator.IsValid(GetSettings()));
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.Received().Exists(_DefaultTargetPath);
            mockHelper.IDirectoryMock.Received().Exists(_DefaultLogPath);
        }

        [Fact]
        public void CanStart_LogPathNotSet_IsAllowed()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Exists(_DefaultSourcePath).Returns(true);
            mockHelper.IDirectoryMock.Exists(_DefaultTargetPath).Returns(true);

            SettingsValidator settingsValidator = new SettingsValidator(mockHelper.IDirectoryMock);
            settingsValidator.IsValid(GetSettings(logPath: string.Empty));
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.Received().Exists(_DefaultTargetPath);
            mockHelper.IDirectoryMock.VerifyNoOtherCalls(new System.Collections.Generic.List<string>() { "Exists" });
        }
    }
}
