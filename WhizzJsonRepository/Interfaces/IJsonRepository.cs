using System.Collections.Immutable;
using Npgsql;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Interfaces
{
    public interface IJsonRepository
    {
        IDatabase Db { get; }
        NpgsqlConnection Connection { get; }
        string[] SelectColumnNames { get; }
        public string QuotedRelationName { get; }
        RelationEntity RelationSchema { get; }
        ImmutableArray<ColumnEntity> ColumnSchema { get; }
        ImmutableArray<ColumnEntity> PrimaryKeyColumnSchema { get; }
    }
}