using System.Collections.Generic;
using System.Collections.Immutable;

namespace WhizzSchema.Entities
{
    public class UniqueIndexEntity
    {
        public string SchemaName { get; set; }
        public string RelationName { get; set; }
        public string IndexName { get; set; }
        public ImmutableArray<string> ColumnNames { get; set; }
    }
}
