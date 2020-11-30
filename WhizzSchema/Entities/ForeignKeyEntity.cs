namespace WhizzSchema.Entities
{
    public class ForeignKeyEntity
    {
        public ForeignKeyEntity(string schemaName, string tableName, string columnName, string constraintName, string primaryKeySchemaName, string primaryKeyTableName, string primaryKeyColumnName)
        {
            SchemaName = schemaName;
            TableName = tableName;
            ColumnName = columnName;
            ConstraintName = constraintName;
            PrimaryKeySchemaName = primaryKeySchemaName;
            PrimaryKeyTableName = primaryKeyTableName;
            PrimaryKeyColumnName = primaryKeyColumnName;
        }

        public string SchemaName { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public string ConstraintName { get; }
        public string PrimaryKeySchemaName { get; }
        public string PrimaryKeyTableName { get; }
        public string PrimaryKeyColumnName { get; }
    }
}
