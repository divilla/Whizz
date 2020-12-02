using System;
using System.Collections.Immutable;
using Npgsql;
using WhizzORM.Context;
using WhizzSchema.Interfaces;

namespace WhizzORM.Interfaces
{
    public interface IDbContext
    {
        NpgsqlConnection Connection { get; }
        IDbSchema DbSchema { get; }
        ImmutableDictionary<Type, EntitySchema> EntitySchema { get; }
    }
}