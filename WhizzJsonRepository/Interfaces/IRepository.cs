using System.Threading.Tasks;
using Npgsql;
using WhizzJsonRepository.Repository;
using WhizzJsonRepository.Services;

namespace WhizzJsonRepository.Interfaces
{
    public interface IRepository
    {
        PgDatabase Db { get; }
        NpgsqlConnection Connection { get; set; }
        string RelationName { get; }
        string SchemaName { get; }
        string[] PrimaryKey { get; }
        FindJsonInvoker Find();
        void OpenConnection();
        Task OpenConnectionAsync();
        void CloseConnection();
        Task CloseConnectionAsync();
    }
}