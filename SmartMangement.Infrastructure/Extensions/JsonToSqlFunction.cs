using Microsoft.EntityFrameworkCore.Query;

namespace SmartManagement.Infrastructure.Extensions
{
    public static class JsonToSqlFunction
    {
        public static string JsonValue(string column, [NotParameterized] string path)
        {
            throw new NotSupportedException();
        }
    }
}
