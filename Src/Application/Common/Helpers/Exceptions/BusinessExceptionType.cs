using System.ComponentModel;

namespace Application.Common.Helpers.Exceptions
{
    public enum BusinessExceptionType
    {
        [Description("Uncontrolled business exception")]
        UncontrolledBusinessException = 001,

        [Description("A problem occurred while saving information in the database.")]
        ProblemSavingDatabase = 002,

        [Description("A problem occurred while getting the information in the database.")]
        ProblemGetData = 003,

        [Description("A problem occurred while updating the information in the database.")]
        ProblemUpdateDatabase = 004,

        [Description("A problem occurred while deleting the information in the database.")]
        ProblemDeleteInfoDatabase = 005,
    }
}
