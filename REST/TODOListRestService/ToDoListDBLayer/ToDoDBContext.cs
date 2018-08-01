using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListRestAPIDataModel.DataModel;

namespace ToDoListDBLayer
{
    public class ToDoDBContext : DbContext
    {
        public virtual DbSet<ToDoTask> ToDoTasks { get; set; }
        public virtual DbSet<ToDoList> ToDoLists { get; set; }
    }
}
