using System.Collections.Generic;

namespace ToDoListRestAPIDataModel.Helpers
{
    public static class ResponseStatusCodeHelper
    {
        public static readonly Dictionary<int, string> GETStatusCodeMap = new Dictionary<int, string>()
        {
            { 200, "successful operation" }, //HttpStatusCode.OK
            { 400, "Invalid id supplied" }, //HttpStatusCode.BadRequest
            { 404, "List not found" }, //HttpStatusCode.NotFound
        };

        public static readonly Dictionary<int, string> POSTStatusCodeMap = new Dictionary<int, string>()
        {
            { 201, "item created/updated" }, //HttpStatusCode.Created
            { 400, "invalid input, object invalid" }, //HttpStatusCode.BadRequest
            { 409, "an existing item already exists" }, //HttpStatusCode.Conflict
            { 404, "not found" }, //HttpStatusCode.NotFound
        };

        public static string GetGETDescription(int code)
        {
            if (GETStatusCodeMap.ContainsKey(code))
            {
                return GETStatusCodeMap[code];
            }
            return string.Empty;
        }
    }
}
