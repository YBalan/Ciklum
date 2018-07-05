using System.Web.Script.Serialization;

namespace ToDoListRestAPIDataModel.Helpers
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
