using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{   
    [DataContract]
    public abstract class ToDoListEntityBase : IToDoListEntity
    {
        public ToDoListEntityBase()
        {
            Id = Guid.NewGuid();
        }

        [DataMember(Name="id")]
        public virtual Guid Id { get; set; }

        [DataMember(Name="name")]
        public virtual string Name { get; set; }

        public bool Validate()
        {
            return Id != null && !string.IsNullOrEmpty(Name);
        }
    }
}