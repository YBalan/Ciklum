﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class Task : Base
    {
        [DataMember]
        public bool Completed { get; set; }
    }
}