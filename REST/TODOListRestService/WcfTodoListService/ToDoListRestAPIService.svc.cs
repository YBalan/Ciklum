using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using ToDoListRestAPIDataModel.DataModel;

using SCHelper = ToDoListRestAPIDataModel.Helpers.ResponseStatusCodeHelper;

namespace WcfTodoListService
{
    public sealed class ToDoListRestAPIService : IToDoListRestAPIService
    {
        private void FillResponse(int code, Dictionary<int, string> map)
        {
            WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound();
            if (Enum.IsDefined(typeof(HttpStatusCode), code))
            {
                var statusCode = (HttpStatusCode)code;
                FillResponse(statusCode, map);
            }
        }

        private void FillResponse(HttpStatusCode statusCode, Dictionary<int, string> map)
        {
            WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound();
            int code = (int)statusCode;
            if (map.ContainsKey(code))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = map[code];
            }
        }

        #region GET Methods
        //http://wcftodolistservice20180706114723.azurewebsites.net/ToDoListRestAPIService.svc/lists
        public IEnumerable<TodoList> GetLists()
        {
            FillResponse(HttpStatusCode.OK, SCHelper.GETStatusCodeMap);
            var result = Persistence.Instance.GetLists();
            return result;
        }

        public TodoList GetList(string id)
        {
            var result = Persistence.Instance.GetList(id);
            FillResponse(result == null ? HttpStatusCode.NotFound : HttpStatusCode.OK, SCHelper.GETStatusCodeMap);
            return result;
        }
        #endregion

        #region POST Methods
        public void AddNewList(Stream data)
        {
            var statusCode = Persistence.Instance.AddNewList(data);
            FillResponse(statusCode, SCHelper.POSTStatusCodeMap);
        }

        public void AddNewTask(string listId, Stream data)
        {
            var statusCode = Persistence.Instance.AddNewTask(listId, data);
            FillResponse(statusCode, SCHelper.POSTStatusCodeMap);
        }

        public void TaskComplete(string listId, string taskId, Stream data)
        {
            var statusCode = Persistence.Instance.TaskComplete(listId, taskId, data);
            FillResponse(statusCode, SCHelper.POSTStatusCodeMap);
        }
        #endregion
    }
}
