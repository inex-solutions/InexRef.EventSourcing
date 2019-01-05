using System.Data;
using System.Data.SqlClient;

namespace InexRef.EventSourcing.Persistence.SqlServer.Utils
{
    public class StringSqlParameterCreator : ISqlParameterCreator<string>
    {
        public SqlParameter Create(string name, string value)
            => new SqlParameter(name, SqlDbType.NVarChar)
            {
                Value = value
            };
    }
}