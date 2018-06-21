using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class RESTAPIExceptionData
    {
        [DataMember]
        public string RESTMessage { get; set; }

        public RESTAPIExceptionData(string message)
        {
            RESTMessage = message;
        }
    }
}