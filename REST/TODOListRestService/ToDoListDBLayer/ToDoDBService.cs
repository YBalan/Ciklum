using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListRestAPIDataModel.DataModel;

namespace ToDoListDBLayer
{
    public sealed class ToDoDBService
    {
        private ToDoDBContext _context;

        public ToDoDBService(ToDoDBContext context)
        {
            _context = context;
        }

        public void AddList(ToDoList list)
        {
            _context.ToDoLists.Add(list);
            _context.SaveChanges();            
        }

        public List<ToDoList> GetAllToDoLists()
        {
            return _context.ToDoLists.Select(l => l).OrderBy(l => l.Name).ToList();
        }

        public List<ToDoTask> GetAllToDoTasks()
        {
            return _context.ToDoTasks.Select(l => l).OrderBy(l => l.Name).ToList();
        }

        public void StoreToDB(Persistence persistence)
        {
            foreach (var list in persistence.GetLists())
            {
                AddList(list);
            }
        }
    }
}
