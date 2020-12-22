using System.Collections.Immutable;

namespace WhizzSchema.Entities
{
    public class RelationSchema
    {
        public RelationSchema(
            string relationName, 
            string schemaName, 
            string quotedSchemaRelation, 
            ImmutableArray<ColumnEntity> columns,
            ImmutableArray<string> columnNames,
            ImmutableArray<ColumnEntity> primaryKey,
            ImmutableArray<string> primaryKeyNames,
            ImmutableArray<UniqueIndexEntity> uniqueIndexes,
            ImmutableArray<ForeignKeyEntity> foreignKeys
        )
        {
            RelationName = relationName;
            SchemaName = schemaName;
            QuotedSchemaRelation = quotedSchemaRelation;
            Columns = columns;
            ColumnNames = columnNames;
            PrimaryKey = primaryKey;
            PrimaryKeyNames = primaryKeyNames;
            UniqueIndexEntities = uniqueIndexes;
            ForeignKeys = foreignKeys;
        }

        public string RelationName { get; }
        public string SchemaName { get; }
        public string QuotedSchemaRelation { get; }
        public ImmutableArray<ColumnEntity> Columns { get; } 
        public ImmutableArray<string> ColumnNames { get; }
        public ImmutableArray<ColumnEntity> PrimaryKey { get; }
        public ImmutableArray<string> PrimaryKeyNames { get; }
        public ImmutableArray<UniqueIndexEntity> UniqueIndexEntities { get; }
        public ImmutableArray<ForeignKeyEntity> ForeignKeys { get; }
    }
}