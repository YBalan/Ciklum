using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ToDoListRestAPIDataModel.DataModel
{
    public sealed class Persistence
    {
        private static volatile Persistence _instance;
        private ConcurrentBag<ToDoList> TodoLists = new ConcurrentBag<ToDoList>();

        private static readonly object _syncObjectSigletone = new object();

        private Persistence()
        {

        }

        public static Persistence Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObjectSigletone)
                    {
                        if (_instance == null)
                        {
                            _instance = new Persistence();
                        }
                    }
                }

                return _instance;
            }
        }

        #region GET Methods
        public IEnumerable<ToDoList> GetLists()
        {
            return TodoLists;
        }

        public ToDoList GetList(string id)
        {
            return TodoLists.FirstOrDefault(tdl => tdl.Id == ParseGuid(id));
        }
        #endregion

        #region POST Methods
        public Status AddNewList(ToDoList list)
        {
            if (list == null || !list.Validate())
            {
                return Status.Invalid;
            }

            if (TodoLists.Any(tdl => tdl.Id == list.Id))
            {
                return Status.AlreadyExist;
            }

            TodoLists.Add(list);

            return Status.Created;
        }


        public Status AddNewTask(string listId, ToDoTask task)
        {
            if (task == null || string.IsNullOrWhiteSpace(listId) || !task.Validate())
            {
                return Status.Invalid;
            }

            var list = TodoLists.FirstOrDefault(tdl => tdl.Id == ParseGuid(listId));

            if (list == null)
            {
                return Status.NotFound;
            }

            if (list.ToDoTasks.Any(tsk => tsk != null && tsk.Id == task.Id))
            {
                return Status.AlreadyExist;
            }

            list.AddTask(task);

            return Status.Created;
        }

        public Status TaskComplete(string listId, string taskId, bool completed)
        {
            if (string.IsNullOrWhiteSpace(listId) || string.IsNullOrWhiteSpace(taskId))
            {
                return Status.Invalid;
            }

            var task = TodoLists.FirstOrDefault(tdl => tdl.Id == ParseGuid(listId))?.
                       ToDoTasks?.FirstOrDefault(tsk => tsk.Id == ParseGuid(taskId));

            if (task == null)
            {
                return Status.NotFound;
            }

            task.Completed = completed;

            return Status.Created;
        }

        private static Guid ParseGuid(string id)
        {
            Guid.TryParse(id, out Guid res);
            return res;
        }
    }
    #endregion
}

