using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ToDoListRestAPIDataModel.DataModel
{
    public static class JsonParseHelper
    {
        public static TResult DeserializeJson<TResult>(this string json) where TResult : class, new()
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<TResult>(json);
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeJson<TResult>(this TResult obj) where TResult : class, new()
        {   
            try
            {
                return new JavaScriptSerializer().Serialize(obj);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
