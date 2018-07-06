using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{   
    [DataContract]
    public abstract class TodoListEntityBase : ITodoListEntity
    {
        public TodoListEntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        [DataMember(Name="id")]
        public virtual string Id { get; set; }

        [DataMember(Name="name")]
        public virtual string Name { get; set; }
    }
}