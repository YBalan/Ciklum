using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using ToDoListRestAPIDataModel.DataModel;


namespace WcfTodoListService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public sealed class ToDoListRestAPIService : IToDoListRestAPIService
    {
        private BlockingCollection<TodoList> TodoLists = new BlockingCollection<TodoList>();
        
        #region GET Methods
        public IEnumerable<TodoList> GetLists()
        {
            return TodoLists;
        }

        public TodoList GetList(string id)
        {
            return TodoLists.FirstOrDefault(tdl => tdl.id == id);// ?? throw new FaultException(MessageFault.CreateFault(new FaultCode("404"), "List not found"));
        }
        #endregion

        #region POST Methods
        public AddObjectResult AddNewList(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();//BitConverter.ToString(Convert.FromBase64String(data));//reader.ReadToEnd();
                var list = json.DeserializeJson<TodoList>();

                if (list == null)
                {
                    return AddObjectResult.Invalid;
                }

                if (TodoLists.Any(tdl => tdl != null && tdl.id == list.id))
                {
                    return AddObjectResult.Exists;
                }

                TodoLists.TryAdd(list);

                return AddObjectResult.Created;
            }
        }


        public AddObjectResult AddNewTask(string listId, string data)
        {
            //using (var reader = new StreamReader(data))
            {
                var json = BitConverter.ToString(Convert.FromBase64String(data));//reader.ReadToEnd();
                var task = json.DeserializeJson<Task>();

                if (task == null)
                {
                    return AddObjectResult.Invalid;
                }

                var list = TodoLists.FirstOrDefault(tdl=>tdl.id == listId);

                if (list == null)
                {
                    return AddObjectResult.Invalid;                   
                }

                if (list.tasks.Any(tsk => tsk != null && tsk.id == task.id))
                {
                    return AddObjectResult.Exists;
                }

                list.tasks?.Add(task);

                return AddObjectResult.Created;
            }
        }


        public AddObjectResult TaskComplete(string listId, string taskId, string data)
        {
            //using (var reader = new StreamReader(data))
            {
                var json = BitConverter.ToString(Convert.FromBase64String(data));//reader.ReadToEnd();
                var taskComplete = json.DeserializeJson<CompletedTask>();

                if (taskComplete == null)
                {
                    return AddObjectResult.Invalid;
                }

                var task = TodoLists.FirstOrDefault(tdl => tdl != null && tdl.id == listId)?.tasks?.FirstOrDefault(tsk => tsk != null && tsk.id == taskId);                

                if (task == null)
                {
                    return AddObjectResult.Invalid;
                }

                task.completed = taskComplete.Completed;

                return AddObjectResult.Created;
            }
        }

        #endregion
    }
}
