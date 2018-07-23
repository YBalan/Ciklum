using System.ComponentModel;

namespace ToDoListRestAPIDataModel.DataModel
{
    public enum Status
    {
        [Description("successful operation")]
        OK,
        [Description("invalid input, object invalid")]
        Invalid,
        [Description("Not found")]
        NotFound,
        [Description("an existing item already exists")]
        AlreadyExist,
        [Description("Created")]
        Created,
    }
}
