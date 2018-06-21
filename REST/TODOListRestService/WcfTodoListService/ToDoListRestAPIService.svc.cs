﻿using System;
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
    public sealed class ToDoListRestAPIService : IToDoListRestAPIService
    {
        private List<TodoList> TodoLists = new List<TodoList>();
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        #region GET Methods
        public IEnumerable<TodoList> GetLists()
        {
            return TodoLists;
        }

        public TodoList GetList(string id)
        {
            return TodoLists.FirstOrDefault(tdl => tdl.Id == id) ?? throw new FaultException(MessageFault.CreateFault(new FaultCode("404"), "List not found"));
        }
        #endregion

        #region POST Methods
        public AddObjectResult AddNewList(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var list = json.DeserializeJson<TodoList>();

                if (list != null)
                {
                    return AddObjectResult.Invalid;
                }

                if (TodoLists.Any(tdl => tdl.Id == list.Id))
                {
                    return AddObjectResult.Exists;
                }

                TodoLists.Add(list);

                return AddObjectResult.Created;
            }
        }


        public AddObjectResult AddNewTask(string listId, Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var task = json.DeserializeJson<Task>();

                if (task != null)
                {
                    return AddObjectResult.Invalid;
                }

                var list = TodoLists.FirstOrDefault(tdl=>tdl.Id == listId);

                if (list == null)
                {
                    return AddObjectResult.Invalid;                   
                }

                if (list.Tasks.Any(tsk => tsk.Id == task.Id))
                {
                    return AddObjectResult.Exists;
                }

                list.AddTask(task);

                return AddObjectResult.Created;
            }
        }


        public AddObjectResult TaskComplete(string listId, string taskId, Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var taskComplete = json.DeserializeJson<CompletedTask>();

                if (taskComplete != null)
                {
                    return AddObjectResult.Invalid;
                }

                var task = TodoLists.FirstOrDefault(tdl => tdl.Id == listId)?.Tasks.FirstOrDefault(tsk => tsk.Id == taskId);                

                if (task == null)
                {
                    return AddObjectResult.Invalid;
                }

                task.Completed = taskComplete.Completed;

                return AddObjectResult.Created;
            }
        }

        #endregion
    }
}