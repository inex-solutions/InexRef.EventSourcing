using System.Configuration;
using Rob.EventSourcing.Sql.Persistence;

namespace Rob.EventSourcing.Tests
{
    public class ConfigFileSqlServerPersistenceConfiguration : SqlServerPersistenceConfiguration
    {
        public ConfigFileSqlServerPersistenceConfiguration()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Rob.EventSourcing.Test"].ConnectionString;
        }
    }
}