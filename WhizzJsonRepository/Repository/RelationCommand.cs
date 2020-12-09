using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Npgsql;
using WhizzJsonRepository.Interfaces;
using WhizzORM.Interfaces;
using WhizzSchema;
using WhizzSchema.Entities;

namespace WhizzORM.Context
{
    public class RelationCommand<TDbCommand, TData> : BaseCommand<TDbCommand, TData>
    {
        public RelationCommand(IRepository repository, TData data, string relationName, string schemaName = DbSchema.DefaultSchema) : base(repository, data)
        {
            QuotedRelationName = repository.Schema.QuotedRelationName(relationName, schemaName);
            RelationName = relationName;
            SchemaName = schemaName;
            Relation = repository.Schema.GetRelation(relationName, schemaName);
            Columns = repository.Schema.GetColumns(relationName, schemaName);
        }
        
        protected string QuotedRelationName { get; }
        protected string RelationName { get; }
        protected string SchemaName { get; }
        protected RelationEntity Relation { get; }
        protected ImmutableArray<ColumnEntity> Columns { get; }
        
        protected class CacheEntry
        {
            public CacheEntry(Type type, string relationName, string schemaName, bool isJson, NpgsqlCommand command)
            {
                Type = type;
                RelationName = relationName;
                SchemaName = schemaName;
                IsJson = isJson;
                Command = command;
            }

            public Type Type;
            public string RelationName;
            public string SchemaName;
            public bool IsJson;
            public NpgsqlCommand Command;
        }
    }
}