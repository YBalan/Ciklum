using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class PostTest
    {
        private const string REST_SERVICE_START_URL = "http://localhost:8000/ToDoListRestAPIService.svc/";
        [TestMethod]
        public void AddNewListTest()
        {
            try
            {
                var jsonNewList = 
@"{{
  ""id"": ""d290f1ee-6c54-4b01-90e6-d701748f0851"",
  ""name"": ""Home"",
  ""description"": ""The list of things that need to be done at home\n"",
  ""tasks"": [
    {
      ""id"": ""0e2ac84f-f723-4f24-878b-44e63e7ae580"",
      ""name"": ""mow the yard"",
      ""completed"": true
    }
  ]
}";
                var res = HttpClientTestHelper.SendPost2(REST_SERVICE_START_URL + "lists/new", jsonNewList);
            }
            catch (FaultException ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }
    }
}
