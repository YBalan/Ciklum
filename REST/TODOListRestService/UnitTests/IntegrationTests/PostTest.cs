using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using Tests;
using ToDoListRestAPIDataModel.DataModel;
using ToDoListRestAPIDataModel.Helpers;


using TodoListTask = ToDoListRestAPIDataModel.DataModel.Task;
using SCHelper = ToDoListRestAPIDataModel.Helpers.ResponseStatusCodeHelper;

namespace IntegrationTests
{
    [TestClass]
    public sealed class PostTest
    {
        //var jsonNewList =
        //@"{{
        //  ""id"": ""d290f1ee-6c54-4b01-90e6-d701748f0851"",
        //  ""name"": ""Home"",
        //  ""description"": ""The list of things that need to be done at home\n"",
        //  ""tasks"": [
        //    {
        //      ""id"": ""0e2ac84f-f723-4f24-878b-44e63e7ae580"",
        //      ""name"": ""mow the yard"",
        //      ""completed"": true
        //    }
        //  ]
        //}";

        //var jsonNewTask =
        //@"{{
        //  ""id"": ""0e2ac84f-f723-4f24-878b-44e63e7ae580_"",
        //  ""name"": ""mow the yard"",
        //  ""completed"": true
        //}";

        //var jsonCompleteTask =
        //@"{{
        //  ""completed"": true
        //}";

        public static readonly TodoList testList = new TodoList
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Home",
            Description = "The list of things that need to be done at home",
        };

        public static readonly TodoListTask testTask = new TodoListTask
        {
            Id = Guid.NewGuid().ToString(),
            Name = "mow the yard",
            Completed = false,
        };

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            testList.Tasks?.Add(testTask);
        }

        [TestMethod]
        public void AddNewListTest()
        {
            testList.Id = Guid.NewGuid().ToString();
            var jsonNewList = testList.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);            

            Assert.AreEqual(201, (int)statusCode);
            //Assert.AreEqual(SCHelper.GetGETDescription(201), description);


            json = HttpClientTestHelper.SendGet(HttpClientTestHelper.REST_SERVICE_START_URL + "list/" + testList.Id);

            var list = json.DeserializeJson<TodoList>();
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

            Assert.ThrowsException<WebException>(()=>json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + "lists/new",
                                                        jsonNewList,
                                                        out HttpStatusCode statusCode1,
                                                        out string description1));

            //Assert.AreEqual(409, (int)statusCode1);
            //Assert.AreEqual(SCHelper.GetGETDescription(409), description1);
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

            //Assert.AreEqual(400, (int)statusCode);
            //Assert.AreEqual(SCHelper.GetGETDescription(400), description);
        }


        [TestMethod]
        public void AddNewTaskTest()
        {
            AddNewListTest();

            testTask.Id = Guid.NewGuid().ToString();
            var jsonNewTask = testTask.SerializeJson();

            var json = HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"list/{testList.Id}/tasks",
                                                        jsonNewTask,
                                                        out HttpStatusCode statusCode,
                                                        out string description);

            Assert.AreEqual(201, (int)statusCode);
           // Assert.AreEqual(SCHelper.GetGETDescription(201), description);
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
            //Assert.AreEqual(SCHelper.GetGETDescription(201), description);
        }
    }
}
