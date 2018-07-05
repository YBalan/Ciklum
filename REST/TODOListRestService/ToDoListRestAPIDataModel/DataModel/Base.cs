using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{   
    [DataContract]
    public class Base
    {
        public Base()
        {
            Id = Guid.NewGuid().ToString();
        }

        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }
    }
}