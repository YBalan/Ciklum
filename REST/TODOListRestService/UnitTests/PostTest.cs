using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ToDoListRestAPIDataModel.DataModel;

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
                var json = HttpClientTestHelper.SendPost(REST_SERVICE_START_URL + "lists/new", jsonNewList);
                var result = json.DeserializeJson<AddObjectResult>();
                Assert.IsNotNull(result);

                Assert.AreEqual(AddObjectResult.Created.Code, result.Code);
                Assert.AreEqual(AddObjectResult.Created.Description, result.Description);
                

                //json = HttpClientTestHelper.SendGet(REST_SERVICE_START_URL + "list/d290f1ee-6c54-4b01-90e6-d701748f0851");

                //var list = json.DeserializeJson<TodoList>();
                //Assert.IsNotNull(list);

                //Assert.Equals("d290f1ee-6c54-4b01-90e6-d701748f0851", list.Id);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                throw;
            }
        }
    }
}
