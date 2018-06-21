using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ToDoRestAPIService;

namespace UnitTests
{
    [TestClass]
    public class GetTest
    {
        private const string REST_SERVICE_START_URL = "http://localhost:8000/ToDoListRestAPIService.svc/";
        [TestMethod]
        public void GetLists()
        {
            try
            {
                var json = HttpClientTestHelper.Send(REST_SERVICE_START_URL + "lists", HttpMethod.Get);
                var list = HttpClientTestHelper.ParseJson<List<TodoList>>(json);
                Assert.IsNotNull(list);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void GetListPositive()
        {
            try
            {
                var id = "validID"; // TODO
                var json = HttpClientTestHelper.Send(REST_SERVICE_START_URL + "list/" + id, HttpMethod.Get);
                var list = HttpClientTestHelper.ParseJson<TodoList>(json);
                Assert.IsNotNull(list);

            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void GetListNegative()
        {
            try
            {
                var json = HttpClientTestHelper.Send(REST_SERVICE_START_URL + "list/flagkjflgj", HttpMethod.Get);
                var list = HttpClientTestHelper.ParseJson<TodoList>(json);
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
