using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using Tests;
using ToDoListRestAPIDataModel.DataModel;

using TodoListTask = ToDoListRestAPIDataModel.DataModel.ToDoTask;

namespace SystemTests
{
    [TestClass]
    public sealed class PostTest
    {
        public static readonly ToDoList testList = new ToDoList
        {
            Name = "Home",
            Description = "The list of things that need to be done at home",
        };

        public static readonly TodoListTask testTask = new TodoListTask
        {
            Name = "mow the yard",
            Completed = false,
        };

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            testList.AddTask(testTask);
        }

        [TestMethod]
        public void AddEmptyListTest()
        {
            var jsonNewList = @"{}";

            Assert.ThrowsException<WebException>(() => HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description));

        }

        [TestMethod]
        public void AddNewListTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);


            json = HttpClientTestHelper.SendGet(HttpClientTestHelper.REST_SERVICE_START_URL + "list/" + testList.Id);

            var list = json.DeserializeJson<ToDoList>();
            Assert.IsNotNull(list);

            Assert.AreEqual(testList.Id, list.Id);
        }

        [TestMethod]
        public void AddExistListTest()
        {
            var jsonNewList = testList.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + "lists/new",
                                                        jsonNewList,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.ThrowsException<WebException>(() => json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + "lists/new",
                                                        jsonNewList,
                                                        out HttpStatusCode statusCode1,
                                                        out string description1));
        }

        [TestMethod]
        public void AddWrongListTest()
        {
            var jsonNewList =
@"
  ""id"": ""d290f1ee-6c54-4b01-90e6-d701748f0851"",
  ""name"": ""Home"",
  ""description"": ""The list of things that need to be done at home\n"",
  ""tasks"": [
    {
      ""id"": ""0e2ac84f-f723-4f24-878b-44e63e7ae580"",
      ""name"": ""mow the yard"",
      ""completed"": true   
  
}";

            Assert.ThrowsException<WebException>(() => HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + "lists/new",
                                                    jsonNewList,
                                                    out HttpStatusCode statusCode,
                                                    out string description));            
        }


        [TestMethod]
        public void AddNewTaskTest()
        {
            AddNewListTest();

            testTask.Id = Guid.NewGuid();
            var jsonNewTask = testTask.SerializeJson();

            HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.AreEqual(201, (int)statusCode);
        }

        [TestMethod]
        public void CompleteTaskTest()
        {
            AddNewListTest();

            var jsonCompleteTask = new CompletedTask { Completed = true }.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/task/{testTask.Id}/complete",
                                                        jsonCompleteTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.AreEqual(201, (int)statusCode);
        }
    }
}
