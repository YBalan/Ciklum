using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ToDoListRestAPIDataModel.DataModel;

namespace WcfTodoListService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IToDoListRestAPIService
    {
        #region GET Methods
        [OperationContract]
        [WebGet(UriTemplate = "/lists", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<TodoList> GetLists();

        [OperationContract]
        [FaultContract(typeof(RESTAPIExceptionData))]
        [WebGet(UriTemplate = "/list/{id}", ResponseFormat = WebMessageFormat.Json)]
        TodoList GetList(string id);
        #endregion

        #region POST Methods
        [OperationContract]
        [FaultContract(typeof(RESTAPIExceptionData))]
        [WebInvoke(UriTemplate = "/lists/new", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        AddObjectResult AddNewList(Stream data);

        [OperationContract]
        //[FaultContract(typeof(RESTAPIExceptionData))]
        [WebInvoke(UriTemplate = "/list/{listId}/tasks/{data}", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        AddObjectResult AddNewTask(string listId, string data);

        [OperationContract]
        [FaultContract(typeof(RESTAPIExceptionData))]
        [WebInvoke(UriTemplate = "/list/{listId}/task/{taskId}/complete/{data}", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        AddObjectResult TaskComplete(string listId, string taskId, string data);
        #endregion
    }   
}
