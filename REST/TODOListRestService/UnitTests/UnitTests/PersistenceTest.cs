using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoListRestAPIDataModel.DataModel;
using ToDoListRestAPIDataModel.Helpers;

namespace UnitTests
{
    [TestClass]
    public class PersistenceTest
    {
        [TestMethod]
        public void AddNewList()
        {
            var list = new TodoList();
            var jsonNewList = list.SerializeJson();

            var bytes = Encoding.UTF8.GetBytes(jsonNewList);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                Assert.AreEqual(201, Persistence.Instance.AddNewList(webStream.Object.GetResponseStream()));
            }
        }

        [TestMethod]
        public void AddNewTask()
        {
            var list = new TodoList();
            var jsonNewList = list.SerializeJson();            

            var bytes = Encoding.UTF8.GetBytes(jsonNewList);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                Assert.AreEqual(201, Persistence.Instance.AddNewList(webStream.Object.GetResponseStream()));                
            }

            var task = new Task();
            var jsonNewTask = task.SerializeJson();

            bytes = Encoding.UTF8.GetBytes(jsonNewTask);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                Assert.AreEqual(201, Persistence.Instance.AddNewTask(list.Id, webStream.Object.GetResponseStream()));
            }
        }
    }
}
