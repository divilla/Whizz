using System.Collections.Generic;
using System.Collections.Immutable;

namespace WhizzORM.Schema
{
    public class UniqueIndexSchema
    {
        public string SchemaName { get; }
        public string RelationName { get; }
        public string IndexName { get; }
        public ImmutableArray<string> ColumnNames { get; }

        public UniqueIndexSchema(string schemaName, string relationName, string indexName, IEnumerable<string> columnNames)
        {
            SchemaName = schemaName;
            RelationName = relationName;
            IndexName = indexName;
            ColumnNames = columnNames.ToImmutableArray();
        }
    }
}
