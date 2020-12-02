namespace WhizzSchema.Entities
{
    public class RelationEntity
    {
        public RelationEntity(string schemaName, string relationName, string owner, string kind, bool canSelect, bool canInsert, bool canUpdate, bool canDelete, bool isReadOnly)
        {
            SchemaName = schemaName;
            RelationName = relationName;
            Owner = owner;
            Kind = kind;
            CanSelect = canSelect;
            CanInsert = canInsert;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
            IsReadOnly = isReadOnly;
        }

        public string SchemaName { get; }
        public string RelationName { get; }
        public string Owner { get; }
        public string Kind { get; }
        public bool CanSelect { get; }
        public bool CanInsert { get; }
        public bool CanUpdate { get; }
        public bool CanDelete { get; }
        public bool IsReadOnly { get; }
    }
}
