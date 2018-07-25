using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using Tests;
using ToDoListRestAPIDataModel.DataModel;

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

        public static readonly ToDoTask testTask = new ToDoTask
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

            Assert.ThrowsException<WebException>(() => TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description));

        }

        [TestMethod]
        public void AddNewListTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            var json = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);


            json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + testList.Id);

            var list = json.DeserializeJson<ToDoList>();
            Assert.IsNotNull(list);

            Assert.AreEqual(testList.Id, list.Id);
        }

        [TestMethod]
        public void AddExistListTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            var json = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + "lists/new",
                                                        jsonNewList,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.ThrowsException<WebException>(() => json = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + "lists/new",
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

            Assert.ThrowsException<WebException>(() => TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + "lists/new",
                                                    jsonNewList,
                                                    out HttpStatusCode statusCode,
                                                    out string description));
        }


        [TestMethod]
        public void AddNewTaskTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);

            testTask.Id = Guid.NewGuid();
            var jsonNewTask = testTask.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode1,
                                                        out string description1);

            Assert.AreEqual(201, (int)statusCode1);

            var json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + testList.Id);

            var result = json.DeserializeJson<ToDoList>();
            Assert.IsNotNull(result);
            Assert.AreEqual(testList.Id, result.Id);
            Assert.IsTrue(result.Tasks.Any(t => t.Id == testTask.Id));
        }

        [TestMethod]
        public void CompleteTaskTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode1,
                                                            out string description1);

            Assert.AreEqual(201, (int)statusCode1);

            var jsonCompleteTask = new CompletedTask { Completed = true }.SerializeJson();

            var json = TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/task/{testTask.Id}/complete",
                                                        jsonCompleteTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.AreEqual(201, (int)statusCode);

            json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + testList.Id);

            var result = json.DeserializeJson<ToDoList>();
            Assert.IsNotNull(result);
            Assert.AreEqual(testList.Id, result.Id);
            Assert.IsTrue(result.Tasks.First(t => t.Id == testTask.Id).Completed);
        }

        [TestMethod]
        public void WrongCompleteTaskTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode1,
                                                            out string description1);

            Assert.AreEqual(201, (int)statusCode1);

            var jsonCompleteTask = @"{111}";

            Assert.ThrowsException<WebException>(()=> TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/task/{testTask.Id}/complete",
                                                        jsonCompleteTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description));            
        }

        [TestMethod]
        public void AddWrongTaskTest()
        {
            var jsonNewTask = @"{{\""1111Completed\"":false,\""Id\"":\""63a68f77-be9e-49fa-ba0b-f42eb0b025ae\"",\""Name\"":\""mow the yard\""}";

            Assert.ThrowsException<WebException>(() => TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description));


        }

        [TestMethod]
        public void AddEmptyTaskTest()
        {
            var jsonNewTask = @"{}";

            Assert.ThrowsException<WebException>(() => TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description));
        }

        [TestMethod]
        public void AddExistTaskTest()
        {
            testList.Id = Guid.NewGuid();
            var jsonNewList = testList.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            Assert.AreEqual(201, (int)statusCode);

            testTask.Id = Guid.NewGuid();
            var jsonNewTask = testTask.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode2,
                                                        out string description2);

            Assert.ThrowsException<WebException>(() => TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode1,
                                                        out string description1));
            
        }
    }
}
