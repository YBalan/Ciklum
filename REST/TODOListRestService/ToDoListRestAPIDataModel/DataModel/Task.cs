using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    //[DataContract]
    //public sealed class Task : Base
    //{
    //    [DataMember(Name = "completed")]        
    //    public bool Completed { get; set; }
    //}

    public class Task
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool completed { get; set; }
    }
}