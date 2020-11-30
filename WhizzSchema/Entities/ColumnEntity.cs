namespace WhizzSchema.Entities
{
    public class ColumnEntity
    {
        public string SchemaName { get; set; }
        public string RelationName { get; set; }
        public string ColumnName { get; set; }
        public short Position { get; set; }
        public uint TypeOid { get; set; }
        public string DataType { get; set; }
        public string TypeType { get; set; }
        public int Size { get; set; }
        public int Modifier { get; set; }
        public int Dimension { get; set; }
        public int? CharacterMaximumLength { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public string[] EnumValues { get; set; }
        public string DefaultValue { get; set; }
        public bool IsNotNull { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }
        public string ColumnComment { get; set; }
    }
}
