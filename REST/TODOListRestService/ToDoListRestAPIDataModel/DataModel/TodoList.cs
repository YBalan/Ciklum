using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class TodoList : Base
    {
        private List<Task> _tasks = new List<Task>();

        [DataMember]
        public IEnumerable<Task> Tasks { get => _tasks; set => _tasks = value.ToList(); }
        [DataMember]
        public string Description { get; set; }

        public void AddTask(Task tsk)
        {
            _tasks.Add(tsk);
        }
    }
}