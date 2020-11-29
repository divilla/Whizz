using System.Collections.Immutable;
using System.Linq;

namespace WhizzSchema.Schema
{
    public class SchemaSchema
    {
        public string SchemaName { get; }
        public string SchemaOwner { get; }
        public bool CanUse { get; }
        public bool CanCreate { get; }
        public ImmutableSortedDictionary<string, RelationSchema> Relations { get; }

        public SchemaSchema(DbSchema dbSchema, string schemaName)
        {
            var schemaEntity = dbSchema.SchemaEntities.SingleOrDefault(s => s.SchemaName == schemaName);

            SchemaName = schemaName;
            SchemaOwner = schemaEntity.SchemaOwner;
            CanUse = schemaEntity.CanCreate;
            CanCreate = schemaEntity.CanCreate;
            Relations = dbSchema.RelationEntities
                .Where(q => q.SchemaName == schemaName)
                .ToImmutableSortedDictionary(
                    d => d.RelationName,
                    d => new RelationSchema(dbSchema, d.RelationName, d.SchemaName));
        }
    }
}
