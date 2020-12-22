namespace WhizzSchema.Entities
{
    public class ForeignKeyEntity
    {
        public string SchemaName { get; set; }
        public string RelationName { get; set; }
        public string ColumnName { get; set; }
        public string ConstraintName { get; set; }
        public string PrimaryKeySchemaName { get; set; }
        public string PrimaryKeyTableName { get; set; }
        public string PrimaryKeyColumnName { get; set; }
    }
}
