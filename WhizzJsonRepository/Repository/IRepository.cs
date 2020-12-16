using Npgsql;
using WhizzJsonRepository.Services;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Repository
{
    public interface IRepository
    {
        PgDatabase Db { get; }
        PgConnectionManager<PgDatabase> ConnectionManager { get; }
        string RelationName { get; }
        string SchemaName { get; }
        ColumnEntity[] PrimaryKeyColumns { get; }
        NpgsqlConnection Connection { get; }
        FindJsonInvoker Find();
        InsertJsonInvoker InsertInto();
    }
}