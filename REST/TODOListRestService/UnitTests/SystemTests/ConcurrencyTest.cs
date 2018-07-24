using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;
using ToDoListRestAPIDataModel.DataModel;

namespace SystemTests
{
    [TestClass]
    public class ConcurrencyTest
    {
        [TestMethod]
        public void AddNewList()
        {
            Parallel.For(0, 10, (i) =>
                {
                    var list = new ToDoList
                    {
                        Name = i.ToString(),
                    };

                    var jsonNewList = list.SerializeJson();

                    TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                             jsonNewList,
                                                             out HttpStatusCode statusCode,
                                                             out string description);

                    Assert.AreEqual(201, (int)statusCode);

                    var json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + list.Id);

                    var result = json.DeserializeJson<ToDoList>();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(list.Id, result.Id);

                });
        }

        [TestMethod]
        public void AddNewTask()
        {
            var list = new ToDoList
            {
                Name = "Test",
            };

            var jsonNewList = list.SerializeJson();

            var json = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);

            Parallel.For(0, 10, (i) =>
            {
                var task = new ToDoTask
                {
                    Name = i.ToString(),
                };

                var jsonNewTask = task.SerializeJson();

                var jsonTask = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{list.Id}/tasks",
                                                            jsonNewTask,
                                                            out HttpStatusCode statusCodeTask,
                                                            out string descriptionTask);

                Assert.AreEqual(201, (int)statusCodeTask);

                json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + list.Id);

                var result = json.DeserializeJson<ToDoList>();
                Assert.IsNotNull(result);
                Assert.AreEqual(list.Id, result.Id);
                Assert.IsTrue(result.Tasks.Any(t => t.Id == task.Id));
            });
        }
    }
}
