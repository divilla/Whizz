using System.Collections.Generic;

namespace WhizzSchema.Entities
{
    public class UniqueIndexEntity
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public IEnumerable<string> ColumnNames { get; set; }
    }
}
