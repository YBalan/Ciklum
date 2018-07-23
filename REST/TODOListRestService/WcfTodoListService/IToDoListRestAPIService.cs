using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using ToDoListRestAPIDataModel.DataModel;

namespace WcfTodoListService
{
    [ServiceContract]
    public interface IToDoListRestAPIService
    {
        #region GET Methods
        [OperationContract]
        [WebGet(UriTemplate = "/lists", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<ToDoList> GetLists();

        [OperationContract]
        [WebGet(UriTemplate = "/list/{id}", ResponseFormat = WebMessageFormat.Json)]
        ToDoList GetList(string id);
        #endregion

        #region POST Methods
        [OperationContract]
        [WebInvoke(UriTemplate = "/lists/new", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void AddNewList(Stream data);

        [OperationContract]
        [WebInvoke(UriTemplate = "/list/{listId}/tasks", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void AddNewTask(string listId, Stream data);

        [OperationContract]
        [WebInvoke(UriTemplate = "/list/{listId}/task/{taskId}/complete", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void TaskComplete(string listId, string taskId, Stream data);
        #endregion
    }
}
