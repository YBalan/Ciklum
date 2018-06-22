using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListRestAPIDataModel.DataModel
{
    public static class JsonParseHelper
    {
        public static TResult DeserializeJson<TResult>(this string json) where TResult : class, new()
        {
            var jsonSerSettings = new DataContractJsonSerializerSettings();            
            var jsonSerializer = new DataContractJsonSerializer(typeof(TResult), jsonSerSettings);
            
            TResult result = null;
            try
            {
                using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    result = jsonSerializer.ReadObject(memStream) as TResult;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return result;
        }

        public static string SerializeJson<TResult>(this TResult obj) where TResult : class, new()
        {
            var jsonSerSettings = new DataContractJsonSerializerSettings();            
            var jsonSerializer = new DataContractJsonSerializer(typeof(TResult), jsonSerSettings);

            var result = string.Empty;
            try
            {                
                using (var memStream = new MemoryStream())
                {
                    jsonSerializer.WriteObject(memStream, obj);
                    result = Encoding.UTF8.GetString(memStream.GetBuffer()).Trim('\0');
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return result;
        }
    }
}
