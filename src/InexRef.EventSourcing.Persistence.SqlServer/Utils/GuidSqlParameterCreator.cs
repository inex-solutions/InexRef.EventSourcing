using System;
using System.Data;
using System.Data.SqlClient;

namespace InexRef.EventSourcing.Persistence.SqlServer.Utils
{
    public class GuidSqlParameterCreator : ISqlParameterCreator<Guid>
    {
        public SqlParameter Create(string name, Guid value)
            => new SqlParameter(name, SqlDbType.UniqueIdentifier)
            {
                Value = value
            };
    }
}