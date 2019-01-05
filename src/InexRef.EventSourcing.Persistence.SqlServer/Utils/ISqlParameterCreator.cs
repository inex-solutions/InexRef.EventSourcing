using System.Data.SqlClient;

namespace InexRef.EventSourcing.Persistence.SqlServer.Utils
{
    public interface ISqlParameterCreator<in TSourceType>
    {
        SqlParameter Create(string name, TSourceType value);
    }
}