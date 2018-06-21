using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfTodoListService.DataModel
{
    [DataContract]
    public sealed class TodoList : Base
    {
        [DataMember]
        public IEnumerable<Task> Tasks { get; } = new List<Task>();
        [DataMember]
        public string Description { get; set; }
    }
}