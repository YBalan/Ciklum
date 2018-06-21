using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using ToDoListRestAPIDataModel.DataModel;
using WcfTodoListService.DataModel;

namespace WcfTodoListService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ToDoListRestAPIService : IToDoListRestAPIService
    {
        private readonly List<TodoList> TodoLists = new List<TodoList>();
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        #region GET Methods
        public IEnumerable<TodoList> GetLists()
        {
            return TodoLists;
        }

        public TodoList GetList(string id)
        {
            return TodoLists.FirstOrDefault(tdl => tdl.Id == id) ?? throw new FaultException(MessageFault.CreateFault(new FaultCode("404"), "List not found"));
        }
        #endregion

        #region POST Methods
        public string AddNewList(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                var json = reader.ReadToEnd();
                var list = json.DeserializeJson<TodoList>();

                if (list != null)
                {
                    throw new FaultException(MessageFault.CreateFault(new FaultCode("400"), "invalid input, object invalid"));
                }

                if (TodoLists.Any(tdl => tdl.Id == list.Id))
                {
                    throw new FaultException(MessageFault.CreateFault(new FaultCode("409"), "an existing item already exists"));
                }

                TodoLists.Add(list);

                return "item created";
            }
        }
        #endregion
    }
}
