using System;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LiveDirectorySyncEngineTests
{
    [TestClass]
    public class RealtimeNoneCacheSyncActionHandlerTests
    {
        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";

        private RealtimeNoneCacheSyncActionHandler GetHandler(IFileSystem fileSystem)
        {
            SyncSettings setings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath);
            return new RealtimeNoneCacheSyncActionHandler(setings, fileSystem);
        }

        [TestMethod]
        public void TestRename_RenameFile_TargetExists()
        {
            FileSystemMockHelper mockHelper = new FileSystemMockHelper();
            mockHelper.FileExists = true;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IFileMock.Verify(a => a.Move(_DefaultTargetPath + "OldName", _DefaultTargetPath + "NewName"));
        }

        [TestMethod]
        public void TestRename_RenameFile_TargetDoesNotExists()
        {
            FileSystemMockHelper mockHelper = new FileSystemMockHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IFileMock.Verify(a => a.Copy(_DefaultSourcePath + "NewName", _DefaultTargetPath + "NewName", true));
        }

        [TestMethod]
        public void TestRename_RenameDirectory_TargetExists()
        {
            FileSystemMockHelper mockHelper = new FileSystemMockHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = true;
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IDirecoryMock.Verify(a => a.Move(_DefaultTargetPath + "OldName", _DefaultTargetPath + "NewName"));
        }

        [TestMethod]
        public void TestRename_RenameDirectory_TargetDoesNotExists()
        {
            FileSystemMockHelper mockHelper = new FileSystemMockHelper();
            mockHelper.FileExists = false;
            mockHelper.DirectoryExists = false;
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncRenameActionCommand command = new SyncRenameActionCommand()
            {
                OldFileName = "OldName",
                NewFileName = "NewName"
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IDirecoryMock.Verify(a => a.Create(_DefaultTargetPath + "NewName"));
        }

        private static void NewMethod(bool fileExists, out Mock<IFileSystem> fileSystem, out Mock<IFile> file)
        {
            fileSystem = new Mock<IFileSystem>();
            file = new Mock<IFile>();
            file.Setup(a => a.Exists(It.IsAny<string>())).Returns(fileExists);

            Mock<IDirectory> directory = new Mock<IDirectory>();
            directory.Setup(a => a.Exists(It.IsAny<string>())).Returns(fileExists);

            fileSystem.SetupGet(b => b.File).Returns(file.Object);
            fileSystem.SetupGet(b => b.Directory).Returns(directory.Object);
            fileSystem.Setup(c => c.IsDirectory(It.IsAny<string>())).Returns(false);
        }
    }

    public class FileSystemMockHelper
    {
        public bool FileExists { get; set; }
        public bool DirectoryExists { get; set; }
        public bool IsDirectory { get; set; }

        public Mock<IFileSystem> IFileSystemMock { get; private set; }
        public Mock<IFile> IFileMock { get; private set; }
        public Mock<IDirectory> IDirecoryMock { get; private set; }

        public void Setup()
        {
            IFileSystemMock = new Mock<IFileSystem>();
            IFileMock = new Mock<IFile>();
            IFileMock.Setup(a => a.Exists(It.IsAny<string>())).Returns(FileExists);

            IDirecoryMock = new Mock<IDirectory>();
            IDirecoryMock.Setup(a => a.Exists(It.IsAny<string>())).Returns(DirectoryExists);

            IFileSystemMock.SetupGet(b => b.File).Returns(IFileMock.Object);
            IFileSystemMock.SetupGet(b => b.Directory).Returns(IDirecoryMock.Object);
            IFileSystemMock.Setup(c => c.IsDirectory(It.IsAny<string>())).Returns(IsDirectory);
        }
    }
}
