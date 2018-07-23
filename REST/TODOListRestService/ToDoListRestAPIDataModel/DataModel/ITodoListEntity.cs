

using System;

namespace ToDoListRestAPIDataModel.DataModel
{
    public interface IToDoListEntity
    {
        Guid Id { get; set; }
        
        string Name { get; set; }
    }
}
