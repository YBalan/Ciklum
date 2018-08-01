using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests;
using ToDoListRestAPIDataModel.DataModel;
using ToDoListDBLayer;
using System.Data.Entity;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class DBCompatibleTest
    {
        [TestMethod]
        public void AddOneList()
        {
            var mockList = new Mock<DbSet<ToDoList>>();

            var mockContext = new Mock<ToDoDBContext>();
            mockContext.Setup(m => m.ToDoLists).Returns(mockList.Object);

            var service = new ToDoDBService(mockContext.Object);
            service.AddList(new ToDoList() { Name = "SomeName", ToDoTasks = new List<ToDoTask> { new ToDoTask { Name = "someTaskName" } } });            

            mockList.Verify(m => m.Add(It.IsAny<ToDoList>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddManyLists()
        {
            var data = new List<ToDoList>
            {
                new ToDoList { Name = "BBB" },
                new ToDoList { Name = "ZZZ" },
                new ToDoList { Name = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ToDoList>>();
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ToDoDBContext>();
            mockContext.Setup(c => c.ToDoLists).Returns(mockSet.Object);

            var service = new ToDoDBService(mockContext.Object);
            var ToDoLists = service.GetAllToDoLists();

            Assert.AreEqual(3, ToDoLists.Count);
            Assert.AreEqual("AAA", ToDoLists[0].Name);
            Assert.AreEqual("BBB", ToDoLists[1].Name);
            Assert.AreEqual("ZZZ", ToDoLists[2].Name);
        }
    }
}
