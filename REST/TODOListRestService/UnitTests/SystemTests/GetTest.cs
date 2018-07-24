using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;
using ToDoListRestAPIDataModel.DataModel;

namespace SystemTests
{
    [TestClass]
    public class GetTest
    {
        public static readonly ToDoList testList = new ToDoList
        {            
            Name = "Home",
            Description = "The list of things that need to be done at home",
        };

        [TestMethod]
        public void GetListsTest()
        {
            var json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "lists");
            var list = json.DeserializeJson<List<ToDoList>>();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListPositiveTest()
        {
            var jsonNewList = testList.SerializeJson();

            TestHelper.SendPost(TestHelper.REST_SERVICE_START_URL + $"lists/new",
                                                            jsonNewList,
                                                            out HttpStatusCode statusCode,
                                                            out string description);

            var id = testList.Id;
            var json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/" + id);
            var list = json.DeserializeJson<ToDoList>();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListNegativeTest()
        {
            var json = TestHelper.SendGet(TestHelper.REST_SERVICE_START_URL + "list/flagkjflgj");
            var list = json.DeserializeJson<ToDoList>();
            Assert.IsNull(list);
        }
    }
}
