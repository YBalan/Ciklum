using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoListRestAPIDataModel.DataModel;

namespace UnitTests
{
    [TestClass]
    public class GetTest
    {
        private const string REST_SERVICE_START_URL = "http://localhost:8000/ToDoListRestAPIService.svc/";
        [TestMethod]
        public void GetListsTest()
        {
            try
            {
                var json = HttpClientTestHelper.SendGet(REST_SERVICE_START_URL + "lists");
                var list = json.DeserializeJson<List<TodoList>>();
                Assert.IsNotNull(list);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void GetListPositiveTest()
        {
            try
            {
                var id = "validID"; // TODO
                var json = HttpClientTestHelper.SendGet(REST_SERVICE_START_URL + "list/" + id);
                var list = json.DeserializeJson<TodoList>();
                Assert.IsNotNull(list);

            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void GetListNegativeTest()
        {
            try
            {
                var json = HttpClientTestHelper.SendGet(REST_SERVICE_START_URL + "list/flagkjflgj");
                var list = json.DeserializeJson<TodoList>();
                Assert.IsNotNull(list);

            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }
    }
}
