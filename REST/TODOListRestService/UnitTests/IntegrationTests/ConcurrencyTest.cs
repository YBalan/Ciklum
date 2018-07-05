using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;
using ToDoListRestAPIDataModel.DataModel;
using ToDoListRestAPIDataModel.Helpers;

using SCHelper = ToDoListRestAPIDataModel.Helpers.ResponseStatusCodeHelper;

namespace IntegrationTests
{
    [TestClass]
    public class ConcurrencyTest
    {
        [TestMethod]
        public void AddNewList()
        {
            Parallel.For(0, 10, (i) =>
                {
                    var list = new TodoList
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = i.ToString(),
                    };

                    var jsonNewList = list.SerializeJson();

                    var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

                    Assert.AreEqual(201, (int)statusCode);
                    //Assert.AreEqual(SCHelper.GetGETDescription(201), description);

                });
        }

        [TestMethod]
        public void AddNewTask()
        {
            var list = new TodoList
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test",
            };

            var jsonNewList = list.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);
            //Assert.AreEqual(SCHelper.GetGETDescription(201), description);

            Parallel.For(0, 10, (i) =>            
            {
                var task = new ToDoListRestAPIDataModel.DataModel.Task
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = i.ToString(),
                };

                var jsonNewTask = task.SerializeJson();

                var jsonTask = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"list/{list.Id}/tasks",
                                                            jsonNewTask,
                                                            out HttpStatusCode statusCodeTask,
                                                            out string descriptionTask);

                Assert.AreEqual(201, (int)statusCodeTask);
                //Assert.AreEqual(SCHelper.GetGETDescription(201), descriptionTask);

            });
        }
    }
}
