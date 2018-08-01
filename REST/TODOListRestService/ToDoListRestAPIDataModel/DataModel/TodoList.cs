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
            ToDoTasks = new List<ToDoTask>();
        }

        public int ToDoListID { get; set; }

        [DataMember(Name = "todotasks")]
        public IList<ToDoTask> ToDoTasks { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public void AddTask(ToDoTask task)
        {
            ToDoTasks.Add(task);
        }
    }    
}