using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListRestAPIDataModel.Helpers;

namespace ToDoListRestAPIDataModel.DataModel
{
    public sealed class Persistence
    {
        private static volatile Persistence _instance;
        private IList<TodoList> TodoLists = new List<TodoList>();

        private static readonly object _syncObjectSigletone = new object();
        private readonly object _syncObjectAddNewList = new object();
        private readonly object _syncObjectAddNewTask = new object();

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
        public IEnumerable<TodoList> GetLists()
        {
            return TodoLists;
        }

        public TodoList GetList(string id)
        {
            return TodoLists.FirstOrDefault(tdl => tdl.Id == id);
        }
        #endregion

        #region POST Methods
        public int AddNewList(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var list = json.DeserializeJson<TodoList>();

                if (list == null)
                {
                    return 400;
                }

                if (TodoLists.Any(tdl => tdl != null && tdl.Id == list.Id))
                {
                    return 409;
                }

                lock (_syncObjectAddNewList)
                {
                    TodoLists.Add(list);
                }

                return 201;
            }
        }


        public int AddNewTask(string listId, Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var task = json.DeserializeJson<Task>();

                if (task == null)
                {
                    return 400;
                }

                var list = TodoLists.FirstOrDefault(tdl => tdl != null && tdl.Id == listId);

                if (list == null)
                {
                    return 400;
                }

                if (list.Tasks.Any(tsk => tsk != null && tsk.Id == task.Id))
                {
                    return 409;
                }

                lock (_syncObjectAddNewTask)
                {
                    list.AddTask(task);
                }

                return 201;
            }
        }


        public int TaskComplete(string listId, string taskId, Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var taskComplete = json.DeserializeJson<CompletedTask>();

                if (taskComplete == null)
                {
                    return 400;
                }

                var task = TodoLists.FirstOrDefault(tdl => tdl != null && tdl.Id == listId)?.
                           Tasks?.FirstOrDefault(tsk => tsk != null && tsk.Id == taskId);

                if (task == null)
                {
                    return 400;
                }

                task.Completed = taskComplete.Completed;

                return 201;
            }
        }

        #endregion

    }
}
