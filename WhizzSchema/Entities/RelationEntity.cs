namespace WhizzSchema.Entities
{
    public class RelationEntity
    {
        public string SchemaName { get; set; }
        public string RelationName { get; set; }
        public string RelationOwner { get; set; }
        public string RelationKind { get; set; }
        public bool CanSelect { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
