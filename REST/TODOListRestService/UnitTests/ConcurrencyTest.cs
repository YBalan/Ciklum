using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoListRestAPIDataModel.DataModel;

namespace UnitTests
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

                    var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new", jsonNewList);
                    var result = json.DeserializeJson<AddObjectResult>();
                    Assert.IsNotNull(result);

                    Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
                    Assert.AreEqual(AddObjectResult.Created.Description, result.Description);

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

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new", jsonNewList);
            var result = json.DeserializeJson<AddObjectResult>();
            Assert.IsNotNull(result);

            Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
            Assert.AreEqual(AddObjectResult.Created.Description, result.Description);

            Parallel.For(0, 10, (i) =>
            {
                var task = new ToDoListRestAPIDataModel.DataModel.Task
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = i.ToString(),
                };               

                var jsonNewTask = task.SerializeJson();

                var jsonTask = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"list/{list.Id}/tasks", jsonNewTask);

                var resultTask = jsonTask.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Created.Description, result.Description);

            });
        }
    }
}
