using System;

namespace WhizzBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : BaseAttribute
    {
        public ForeignKeyAttribute(string tableName, string columnName)
        {
            TableName = tableName;
            ColumnName = columnName;
        }

        public string TableName { get; }
        public string ColumnName { get; }
    }
}
