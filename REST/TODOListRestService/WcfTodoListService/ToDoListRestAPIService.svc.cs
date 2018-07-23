using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using ToDoListRestAPIDataModel.DataModel;

namespace WcfTodoListService
{
    public sealed class ToDoListRestAPIService : IToDoListRestAPIService
    {
        #region GET Methods
        //http://wcftodolistservice20180706114723.azurewebsites.net/ToDoListRestAPIService.svc/lists
        public IEnumerable<ToDoList> GetLists()
        {
            FillResponse(Status.OK);
            var result = Persistence.Instance.GetLists();
            return result;
        }

        public ToDoList GetList(string id)
        {
            var result = Persistence.Instance.GetList(id);
            FillResponse(result == null ? Status.NotFound : Status.OK);
            return result;
        }
        #endregion

        #region POST Methods
        public void AddNewList(Stream data)
        {
            var list = ReadStream<ToDoList>(data);
            var statusCode = Persistence.Instance.AddNewList(list);
            FillResponse(statusCode);
        }

        public void AddNewTask(string listId, Stream data)
        {
            var task = ReadStream<ToDoTask>(data);
            var statusCode = Persistence.Instance.AddNewTask(listId, task);
            FillResponse(statusCode);
        }

        public void TaskComplete(string listId, string taskId, Stream data)
        {
            var completedTask = ReadStream<CompletedTask>(data);
            var statusCode = Persistence.Instance.TaskComplete(listId, taskId, completedTask.Completed);
            FillResponse(statusCode);
        }
        #endregion

        private void FillResponse(Status code)
        {
            var result = HttpStatusCode.NotFound;
            switch (code)
            {
                case Status.OK:
                    result = HttpStatusCode.OK;
                    break;
                case Status.Invalid:
                    result = HttpStatusCode.BadRequest;
                    break;
                case Status.NotFound:
                    result = HttpStatusCode.NotFound;
                    break;
                case Status.AlreadyExist:
                    result = HttpStatusCode.Conflict;
                    break;
                case Status.Created:
                    result = HttpStatusCode.Created;
                    break;
                default:
                    result = HttpStatusCode.BadRequest;
                    break;
            }

            WebOperationContext.Current.OutgoingResponse.StatusCode = result;
        }

        private TResult ReadStream<TResult>(Stream stream) where TResult : class, new()
        {
            try
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return new JavaScriptSerializer().Deserialize<TResult>(json);
                }
            }
            catch
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
        }

    }
}
