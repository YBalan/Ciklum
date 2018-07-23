using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests;
using ToDoListRestAPIDataModel.DataModel;

namespace UnitTests
{
    [TestClass]
    public class PersistenceTest
    {
        [TestMethod]
        public void AddNewList()
        {
            var list = new ToDoList();
            var jsonNewList = list.SerializeJson();

            var bytes = Encoding.UTF8.GetBytes(jsonNewList);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                //Assert.AreEqual(201, Persistence.Instance.AddNewList(webStream.Object.GetResponseStream()));
            }
        }

        [TestMethod]
        public void AddNewTask()
        {
            var list = new ToDoList();
            var jsonNewList = list.SerializeJson();            

            var bytes = Encoding.UTF8.GetBytes(jsonNewList);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                //Assert.AreEqual(201, Persistence.Instance.AddNewList(webStream.Object.GetResponseStream()));                
            }

            var task = new ToDoTask();
            var jsonNewTask = task.SerializeJson();

            bytes = Encoding.UTF8.GetBytes(jsonNewTask);

            using (var stream = new MemoryStream(bytes))
            {
                var webStream = new Mock<HttpWebResponse>();
                webStream.Setup(s => s.GetResponseStream()).Returns(stream);

                //Assert.AreEqual(201, Persistence.Instance.AddNewTask(list.Id.ToString(), webStream.Object.GetResponseStream()));
            }
        }
    }
}
