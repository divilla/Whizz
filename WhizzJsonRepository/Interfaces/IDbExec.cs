using Npgsql;
using WhizzORM.Context;
using WhizzSchema.Interfaces;

namespace WhizzORM.Interfaces
{
    public interface IDbExec<TDbContext> where TDbContext : IDbContext
    {
        TDbContext Context { get; }
        NpgsqlConnection Connection { get; }
        IDbSchema schema { get; }
        InsertCommand<TData> Insert<TData>(string relationName, TData data);
        InsertCommand<TData> Insert<TData>(string relationName, string schemaName, TData data);
    }
}