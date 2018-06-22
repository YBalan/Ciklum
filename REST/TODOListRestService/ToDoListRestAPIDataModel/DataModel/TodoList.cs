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
        public TodoList()
        {
            Tasks = new List<Task>();
        }

        [DataMember(Name = "tasks")]
        public List<Task> Tasks { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public void AddTask(Task tsk)
        {
            Tasks.Add(tsk);
        }
    }

    //public class TodoList
    //{
    //    public TodoList()
    //    {
    //        tasks = new List<Task>();
    //    }
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string description { get; set; }
    //    public List<Task> tasks { get; set; }

    //    //public void AddTask(Task tsk)
    //    //{
    //    //    if (tasks == null) tasks = new List<Task>();
    //    //    tasks.Add(tsk);
    //    //}
    //}
}