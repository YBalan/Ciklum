﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListRestAPIDataModel.DataModel
{
    [DataContract]
    public sealed class CompletedTask
    {
        [DataMember(Name = "completed")]
        public bool Completed { get; set; }
    }
}
