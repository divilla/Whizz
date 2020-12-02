using System;
using System.Reflection;
using WhizzSchema.Entities;

namespace WhizzORM.Context
{
    public class PropertySchema
    {
        public PropertySchema(string propertyName, PropertyInfo propInfo, string columnName, ColumnEntity columnSchema)
        {
            PropertyName = propertyName;
            PropInfo = propInfo;
            ColumnName = columnName;
            ColumnSchema = columnSchema;
        }

        public string PropertyName { get; }
        public PropertyInfo PropInfo { get; }

        public string ColumnName { get; }
        public ColumnEntity ColumnSchema { get; }
    }
}