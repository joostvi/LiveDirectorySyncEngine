using LiveDirectorySyncEngineLogic.Generic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using LiveDirectorySyncEngineLogic.Generic.Model;
using Moq;
using System;
using xunit;

namespace LiveDirectorySyncEngineTests.UnitTests.Generic
{
    [Collection("BaseRepostoryTests")]
    public class BaseRepostoryTests
    {
        private class TestModel : ModelBase
        {
            public TestModel(int id) : base(id)
            {
            }
        }

        private class TestRepository : BaseRepository<TestModel>
        {
            public TestRepository(IDBConnection connection) : base(connection)
            {
            }
        }

        [Fact]
        public void Log_Constructor_Test()
        {
            Assert.ThrowsException<System.NullReferenceException>(() => new TestRepository(null));
        }

        [Fact]
        public void Log_GetAll_Test()
        {
            //setup
            Mock<IDBConnection> connection = new Mock<IDBConnection>();
            TestRepository baseRepostory = new TestRepository(connection.Object);
           
            //act
            baseRepostory.GetAll();

            //verify
            connection.Verify(a => a.GetAll<TestModel>());
        }

        [Fact]
        public void Log_Get_Test()
        {
            //setup
            Mock<IDBConnection> connection = new Mock<IDBConnection>();
            TestRepository baseRepostory = new TestRepository(connection.Object);

            //act
            int id = 123;
            baseRepostory.Get(id);

            //verify
            connection.Verify(a => a.Get<TestModel>(id));
        }

        [Fact]
        public void Log_Store_Test()
        {
            //setup
            Mock<IDBConnection> connection = new Mock<IDBConnection>();
            TestRepository baseRepostory = new TestRepository(connection.Object);

            //act
            int id = 123;
            TestModel testModel = new TestModel(id);
            baseRepostory.Store(testModel);

            //verify
            connection.Verify(a => a.Store<TestModel>(testModel));
        }
    }
}
