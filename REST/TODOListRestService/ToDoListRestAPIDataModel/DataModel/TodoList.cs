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
    public sealed class TodoList : TodoListEntityBase
    {
        public TodoList()
        {            
            Tasks = new List<Task>();
        }

        [DataMember(Name = "tasks")]
        public List<Task> Tasks { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public void AddTask(Task task)
        {
            Tasks.Add(task as Task);
        }
    }    
}