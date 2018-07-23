using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class ToDoTask : ToDoListEntityBase
    {
        [DataMember(Name = "completed")]
        public bool Completed { get; set; }
    }
}