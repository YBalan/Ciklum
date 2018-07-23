using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class ToDoList : ToDoListEntityBase
    {
        public ToDoList()
        {            
            Tasks = new List<ToDoTask>();
        }

        [DataMember(Name = "tasks")]
        public List<ToDoTask> Tasks { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public void AddTask(ToDoTask task)
        {
            Tasks.Add(task as ToDoTask);
        }
    }    
}