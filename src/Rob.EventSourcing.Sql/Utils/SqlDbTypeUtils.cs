using System;
using System.Data;

namespace Rob.EventSourcing.Sql.Utils
{
    public static class SqlDbTypeUtils
    {
        public static SqlDbType GetSqlDbType<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return SqlDbType.Int;
            }

            if (typeof(T) == typeof(long))
            {
                return SqlDbType.BigInt;
            }

            if (typeof(T) == typeof(string))
            {
                return SqlDbType.NVarChar;
            }

            if (typeof(T) == typeof(Guid))
            {
                return SqlDbType.UniqueIdentifier;
            }

            throw new NotSupportedException($"Type {typeof(T).Name} is not supported");
        }
    }
}