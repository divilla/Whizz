using System;
using System.Collections.Immutable;
using WhizzSchema.Entities;
using WhizzSchema.Schema;

namespace WhizzSchema.Interfaces
{
    public interface IDbSchema
    {
        string DatabaseName { get; }
        ImmutableArray<SchemaEntity> SchemaEntities { get; }
        ImmutableArray<RelationEntity> RelationEntities { get; }
        ImmutableArray<ColumnEntity> ColumnEntities { get; }
        ImmutableArray<UniqueIndexEntity> UniqueIndexEntities { get; }
        ImmutableDictionary<string, ImmutableArray<Type>> TypeMap { get; }
        public ImmutableArray<string> UsingNamespaces { get; }
        ImmutableSortedDictionary<string, SchemaSchema> Schemas { get; }
        ImmutableSortedSet<string> Keywords { get; }
        bool SchemaExists(string schemaName);
        bool RelationExists(string relationName, string schemaName);
        RelationSchema GetRelationSchema(string relationName, string schemaName);
        string GetQualifiedRelationName(string relationName, string schemaName);
        string GetEscapedRelationName(string relationName, string schemaName);
        string Quote(string value);
        string EscapedQuote(string value);
    }
}