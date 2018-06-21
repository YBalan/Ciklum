using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class AddObjectResult
    {
        public static AddObjectResult Created = new AddObjectResult { Code = 201, Description = "item created/updated" };
        public static AddObjectResult Invalid = new AddObjectResult { Code = 400, Description = "invalid input, object invalid" };
        public static AddObjectResult Exists = new AddObjectResult { Code = 409, Description = "an existing item already exists" };

        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
