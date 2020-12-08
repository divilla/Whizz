using System;
using System.Collections.Immutable;
using WhizzSchema.Entities;

namespace WhizzSchema.Interfaces
{
    public interface IDbSchema
    {
        const string DefaultSchema = "public";
        string DatabaseName { get; }
        ImmutableArray<SchemaEntity> SchemaEntities { get; }
        ImmutableArray<RelationEntity> RelationEntities { get; }
        ImmutableArray<ColumnEntity> ColumnEntities { get; }
        ImmutableArray<ForeignKeyEntity> ForeignKeyEntities { get; }
        ImmutableArray<UniqueIndexEntity> UniqueIndexEntities { get; }
        ImmutableDictionary<uint, TypeEntity> TypeEntities { get; }
        ImmutableDictionary<string, ImmutableArray<Type>> TypeMap { get; }
        ImmutableArray<string> UsingNamespaces { get; }
        ImmutableSortedSet<string> Keywords { get; }
        bool SchemaExists(string schemaName);
        bool RelationExists(string relationName, string schemaName);
        string QuotedRelationName(string relationName, string schemaName);
        RelationEntity UnquoteRelationName(string name);
        string Unquote(string value);
        string Quote(string value);
        string EscapedQuotedRelationName(string relationName, string schemaName);
        string EscapedQuote(string value);
        RelationEntity GetRelation(string relationName, string schemaName = DefaultSchema);
        ImmutableArray<ColumnEntity> GetColumns(string relationName, string schemaName = DefaultSchema);
        ImmutableArray<ForeignKeyEntity> GetForeignKeys(string tableName, string schemaName = DefaultSchema);
        ImmutableArray<UniqueIndexEntity> GetUniqueIndexes(string tableName, string schemaName = DefaultSchema);
        TypeEntity GetType(ColumnEntity column);
        string GetTypeName(ColumnEntity column);
        ImmutableArray<string> GetColumnNames(string relationName, string schemaName = DefaultSchema);
        ImmutableDictionary<string, Type> GetColumnTypes(string relationName, string schemaName = DefaultSchema);
    }
}