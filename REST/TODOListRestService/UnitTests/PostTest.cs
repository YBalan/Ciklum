using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ToDoListRestAPIDataModel.DataModel;

using TodoListTask = ToDoListRestAPIDataModel.DataModel.Task;

namespace UnitTests
{
    [TestClass]
    public sealed class PostTest
    {
        private const string REST_SERVICE_START_URL = "http://localhost:8000/ToDoListRestAPIService.svc/";

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
            try
            {
                var jsonNewList = testList.SerializeJson();

                //var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + $"lists/{jsonNewList}", jsonNewList);
                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + $"lists/new", jsonNewList);
                var result = json.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Created.Description, result.Description);


                //json = HttpClientTestHelper.SendGet(REST_SERVICE_START_URL + "list/"+testList.Id);

                //var list = json.DeserializeJson<TodoList>();
                //Assert.IsNotNull(list);

                //Assert.Equals(testList.Id, list.Id);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void AddExistListTest()
        {
            try
            {
                var jsonNewList = testList.SerializeJson();

                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + "lists/new", jsonNewList);
                json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + "lists/new", jsonNewList);
                var result = json.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Exists.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Exists.Description, result.Description);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void AddWrongListTest()
        {
            try
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

                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + "lists/new", jsonNewList);
                //var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + $"lists/{jsonNewList}", jsonNewList);
                var result = json.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Invalid.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Invalid.Description, result.Description);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }


        [TestMethod]
        public void AddNewTaskTest()
        {
            try
            {
                AddNewListTest();

                testTask.Id = Guid.NewGuid().ToString();
                var jsonNewTask = testTask.SerializeJson();

                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + $"list/{testTask.Id}/tasks/{jsonNewTask}", string.Empty);
                var result = json.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Created.Description, result.Description);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void CompleteTaskTest()
        {
            try
            {
                AddNewListTest();

                var jsonCompleteTask = new CompletedTask { Completed = true }.SerializeJson();

                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + $"list/{testTask.Id}/task/{testTask.Id}/complete/{jsonCompleteTask}", string.Empty);
                var result = json.DeserializeJson<CompletedTask>();
                Assert.IsNotNull(result);

                Assert.IsFalse(result.Completed);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }
    }
}
