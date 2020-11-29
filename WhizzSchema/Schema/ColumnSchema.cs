using System;
using System.Collections.Immutable;

namespace WhizzSchema.Schema
{
    public class ColumnSchema
    {
        public string SchemaName { get; }
        public string RelationName { get; }
        public string ColumnName { get; }
        public short Position { get; }
        public uint TypeOid { get; }
        public string DataType { get; }
        public string TypeType { get; }
        public int Size { get; }
        public int Modifier { get; }
        public int Dimension { get; }
        public int? CharacterMaximumLength { get; }
        public int? NumericPrecision { get; }
        public int? NumericScale { get; }
        public string[] EnumValues { get; }
        public string DefaultValue { get; }
        public bool IsNotNull { get; }
        public bool IsGenerated { get; }
        public bool IsPrimaryKey { get; }
        public bool IsRequired { get; }
        public bool IsUpdatable { get; }
        public string ColumnComment { get; }
        public Type ClrType { get; }

        public ColumnSchema(string schemaName, string relationName, string columnName, short position, uint typeOid, string dataType, string typeType, int size, int modifier, int dimension, int? characterMaximumLength, int? numericPrecision, int? numericScale, string[] enumValues, string defaultValue, bool isNotNull, bool isGenerated, bool isPrimaryKey, bool isRequired, bool isUpdatable, string columnComment, Type clrType)
        {
            SchemaName = schemaName;
            RelationName = relationName;
            ColumnName = columnName;
            Position = position;
            TypeOid = typeOid;
            DataType = dataType;
            TypeType = typeType;
            Size = size;
            Modifier = modifier;
            Dimension = dimension;
            CharacterMaximumLength = characterMaximumLength;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            EnumValues = enumValues;
            DefaultValue = defaultValue;
            IsNotNull = isNotNull;
            IsGenerated = isGenerated;
            IsPrimaryKey = isPrimaryKey;
            IsRequired = isRequired;
            IsUpdatable = isUpdatable;
            ColumnComment = columnComment;
            ClrType = clrType;
        }
    }
}
