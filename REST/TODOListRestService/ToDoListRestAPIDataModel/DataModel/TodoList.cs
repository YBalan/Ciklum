using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class TodoList : Base
    {
        private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();

        [DataMember]
        public IEnumerable<Task> Tasks
        {
            get => _tasks.ToArray();
            set
            {
                _tasks = new BlockingCollection<Task>();
                Parallel.ForEach(value, v => _tasks.TryAdd(v));
            }
        }
        [DataMember]
        public string Description { get; set; }

        public void AddTask(Task tsk)
        {
            _tasks.TryAdd(tsk);
        }
    }
}