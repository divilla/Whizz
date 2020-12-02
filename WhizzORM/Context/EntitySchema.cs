using System;
using System.Collections.Immutable;

namespace WhizzORM.Context
{
    public class EntitySchema
    {
        public EntitySchema(Type entityType, string dbName, string relationName, string schemaName, bool readOnly, ImmutableArray<PropertySchema> properties)
        {
            EntityType = entityType;
            DbName = dbName;
            RelationName = relationName;
            SchemaName = schemaName;
            ReadOnly = readOnly;
            Properties = properties;
        }

        public Type EntityType { get; }
        public string DbName { get; }
        public string RelationName { get; }
        public string SchemaName { get; }
        public bool ReadOnly { get; }
        public ImmutableArray<PropertySchema> Properties { get; }
    }
}