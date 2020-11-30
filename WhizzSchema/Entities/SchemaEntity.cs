namespace WhizzSchema.Entities
{
    public class SchemaEntity
    {
        public SchemaEntity(string schemaName, string schemaOwner, bool canUse, bool canCreate)
        {
            SchemaName = schemaName;
            SchemaOwner = schemaOwner;
            CanUse = canUse;
            CanCreate = canCreate;
        }

        public string SchemaName { get; }
        public string SchemaOwner { get; }
        public bool CanUse { get; }
        public bool CanCreate { get; }
    }
}
