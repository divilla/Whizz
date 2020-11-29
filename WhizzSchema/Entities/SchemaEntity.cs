namespace WhizzSchema.Entities
{
    public class SchemaEntity
    {
        public string SchemaName { get; set; }
        public string SchemaOwner { get; set; }
        public bool CanUse { get; set; }
        public bool CanCreate { get; set; }
    }
}
