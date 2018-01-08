using System;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.SyncActionModel;
using LiveDirectorySyncEngineTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDirectorySyncEngineTests
{
    [TestClass]
    public class RealtimeNoneCachedSyncActionHandlerTests
    {
        private const string _DefaultSourcePath = @"c:\source\";
        private const string _DefaultTargetPath = @"c:\target\";

        private RealtimeNoneCachedSyncActionHandler GetHandler(IFileSystem fileSystem)
        {
            SyncSettings setings = new SyncSettings(_DefaultSourcePath, _DefaultTargetPath);
            return new RealtimeNoneCachedSyncActionHandler(setings, fileSystem);
        }

        [TestMethod]
        public void TestRename_RenameFile_TargetExists()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
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
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
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
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IDirectoryMock.Verify(a => a.Move(_DefaultTargetPath + "OldName", _DefaultTargetPath + "NewName"));
        }

        [TestMethod]
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
            GetHandler(mockHelper.IFileSystemMock.Object).Rename(command);
            mockHelper.IDirectoryMock.Verify(a => a.Create(_DefaultTargetPath + "NewName"));
        }

        [TestMethod]
        public void TestCreate_CreateFile_HappyPath()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.Setup();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Create(command);
            mockHelper.IFileMock.Verify(a => a.Copy(_DefaultSourcePath + "NewName", _DefaultTargetPath + "NewName", true));
        }

        [TestMethod]
        public void TestCreate_CreateFile_NoneExistingTargetFolder()
        {
            FileSystemMock fileSystemMock = new FileSystemMock();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "UnknownFolder\\File.txt")
            };
            GetHandler(fileSystemMock).Create(command);

            DirectoryMock directories = (DirectoryMock)fileSystemMock.Directory;
            Assert.IsTrue(directories.Directories.Contains(_DefaultTargetPath + "UnknownFolder"));

            FileMock files = (FileMock)fileSystemMock.File;
            Assert.IsTrue(files.Files.Contains(_DefaultTargetPath + "UnknownFolder\\File.txt"));
        }

        [TestMethod]
        public void TestCreate_CreateDirectory_HappyPath()
        {
            FileSystemMoqHelper mockHelper = new FileSystemMoqHelper();
            mockHelper.IsDirectory = true;
            mockHelper.Setup();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "NewName")
            };
            GetHandler(mockHelper.IFileSystemMock.Object).Create(command);
            mockHelper.IDirectoryMock.Verify(a => a.Create(_DefaultTargetPath + "NewName"));
        }

        [TestMethod]
        public void TestCreate_CreateDirectory_NoneExistingTargetFolder()
        {
            FileSystemMock fileSystemMock = new FileSystemMock();

            SyncCreateActionCommand command = new SyncCreateActionCommand()
            {
                SourceFile = new SyncFileInfo(_DefaultSourcePath + "UnknownFolder\\Directory")
            };
            GetHandler(fileSystemMock).Create(command);

            DirectoryMock directories = (DirectoryMock)fileSystemMock.Directory;
            Assert.IsTrue(directories.Directories.Contains(_DefaultTargetPath + "UnknownFolder\\Directory"));

            FileMock files = (FileMock)fileSystemMock.File;
            Assert.IsTrue(files.Files.Count == 0);
        }
    }
}
