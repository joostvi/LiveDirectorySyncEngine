using System.IO;
using LiveDirectorySyncEngineLogic;
using GenericClassLibrary.FileSystem;
using GenericClassLibrary.Validation;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using GenericClassLibraryTests.Mocks;
using NSubstitute;
using Xunit;
using GenericNSubstituteTestHelpers;

namespace LiveDirectorySyncEngineTests.UnitTests
{
    [Collection("RealtimeNoneCachedSyncActionHandlerTests")]
    public class RealtimeNoneCachedSyncActionHandlerTests
    {
        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";
        private const string _DefaultLogPath = @"c:\log\";

        private RealtimeNoneCachedSyncActionHandler GetHandler(IFileSystem fileSystem)
        {
            SyncSettings setings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath, GenericClassLibrary.Logging.EnumLogLevel.Info, _DefaultLogPath);
            return new RealtimeNoneCachedSyncActionHandler(setings, fileSystem);
        }

        [Fact]
        public void TestRename_RenameFile_TargetExists()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper
            {
                FileExists = true,
                DirectoryExists = false
            };
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock).Rename(command);
            mockHelper.IFileMock.Received().Move(_DefaultTargetPath + "OldName", _DefaultTargetPath + "NewName");
        }

        [Fact]
        public void TestRename_RenameFile_TargetDoesNotExists()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock).Rename(command);
            mockHelper.IFileMock.Received().Copy(_DefaultSourcePath + "NewName", _DefaultTargetPath + "NewName", true);
        }

        [Fact]
        public void TestRename_RenameDirectory_TargetExists()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = true;
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock).Rename(command);
            mockHelper.IDirectoryMock.Received().Move(_DefaultTargetPath + "OldName", _DefaultTargetPath + "NewName");
        }

        [Fact]
        public void TestRename_RenameDirectory_TargetDoesNotExists()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock).Rename(command);
            mockHelper.IDirectoryMock.Received().Create(_DefaultTargetPath + "NewName");
        }

        [Fact]
        public void TestCreate_CreateFile_HappyPath()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.Setup();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            GetHandler(mockHelper.IFileSystemMock).Create(command);
            mockHelper.IFileMock.Received().Copy(_DefaultSourcePath + "NewName", _DefaultTargetPath + "NewName", true);
        }

        [Fact]
        public void TestCreate_CreateFile_SourceDeleted()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.Setup();
            mockHelper.IFileMock.When(a => a.Copy(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())).Do( a => throw new FileNotFoundException());

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            //this logic should accept file not found
            GetHandler(mockHelper.IFileSystemMock).Create(command);
        }

        [Fact]
        public void TestCreate_UpdateFile_SourceDeleted()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.Setup();
            mockHelper.IFileMock.When(a => a.Copy(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())).Do( a => throw new FileNotFoundException());

            SyncUpdateActionCommand command = new SyncUpdateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            //this logic should accept file not found
            GetHandler(mockHelper.IFileSystemMock).Update(command);
        }

        [Fact]
        public void TestCreate_CreateFile_NoneExistingTargetFolder()
        {
            FileSystemMock fileSystemMock = new FileSystemMock();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "UnknownFolder\\File.txt")
            };
            GetHandler(fileSystemMock).Create(command);

            DirectoryMock directories = (DirectoryMock)fileSystemMock.Directory;
            Assert.True(directories.Directories.Contains(_DefaultTargetPath + "UnknownFolder"));

            FileMock files = (FileMock)fileSystemMock.File;
            Assert.True(files.Files.Contains(_DefaultTargetPath + "UnknownFolder\\File.txt"));
        }

        [Fact]
        public void TestCreate_CreateDirectory_HappyPath()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            GetHandler(mockHelper.IFileSystemMock).Create(command);
            mockHelper.IDirectoryMock.Received().Create(_DefaultTargetPath + "NewName");
        }

        [Fact]
        public void TestCreate_CreateDirectory_NoneExistingTargetFolder()
        {
            FileSystemMock fileSystemMock = new FileSystemMock();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "UnknownFolder\\Directory")
            };
            GetHandler(fileSystemMock).Create(command);

            DirectoryMock directories = (DirectoryMock)fileSystemMock.Directory;
            Assert.True(directories.Directories.Contains(_DefaultTargetPath + "UnknownFolder\\Directory"));

            FileMock files = (FileMock)fileSystemMock.File;
            Assert.True(files.Files.Count == 0);
        }

        [Fact]
        public void CanStart_MissingSourcePath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            var handler = GetHandler(mockHelper.IFileSystemMock);
            Assert.Throws<InvalidInputException>(() => handler.CanStart());
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.VerifyNoOtherCalls(new System.Collections.Generic.List<string>() { "Exists" });
        }

        [Fact]
        public void CanStart_MissingTargetPath_ShouldThrowException()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = true;
            mockHelper.Setup();

            mockHelper.IDirectoryMock.Exists(_DefaultTargetPath).Returns(false);

            var handler = GetHandler(mockHelper.IFileSystemMock);
            Assert.Throws<InvalidInputException>(() => handler.CanStart());
            mockHelper.IDirectoryMock.Received().Exists(_DefaultSourcePath);
            mockHelper.IDirectoryMock.Received().Exists(_DefaultTargetPath);
        }
    }
}
