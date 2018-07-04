using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoListRestAPIDataModel.DataModel;

namespace UnitTests
{
    [TestClass]
    public class GetTest
    {
        public static readonly TodoList testList = new TodoList
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Home",
            Description = "The list of things that need to be done at home",
        };

        [TestMethod]
        public void GetListsTest()
        {
            var json = HttpClientTestHelper.SendGet(HttpClientTestHelper.REST_SERVICE_START_URL + "lists");
            var list = json.DeserializeJson<List<TodoList>>();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListPositiveTest()
        {
            var jsonNewList = testList.SerializeJson();

            HttpClientTestHelper.SendPost(HttpClientTestHelper.REST_SERVICE_START_URL + $"lists/new", jsonNewList);

            var id = testList.Id;
            var json = HttpClientTestHelper.SendGet(HttpClientTestHelper.REST_SERVICE_START_URL + "list/" + id);
            var list = json.DeserializeJson<TodoList>();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListNegativeTest()
        {
            var json = HttpClientTestHelper.SendGet(HttpClientTestHelper.REST_SERVICE_START_URL + "list/flagkjflgj");
            var list = json.DeserializeJson<TodoList>();
            Assert.IsNull(list);
        }
    }
}
