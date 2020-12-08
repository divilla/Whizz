using System;
using WhizzBase.Enums;

namespace WhizzBase.Cache
{
    public class RelationCommandCacheKey
    {
        public RelationCommandCacheKey(SqlCommandType commandTypeType, Type type, string relationName, string schemaName, InsertReturns returns)
        {
            CommandTypeType = commandTypeType;
            Type = type;
            RelationName = relationName;
            SchemaName = schemaName;
            Returns = returns;
        }

        public SqlCommandType CommandTypeType { get; }
        public Type Type { get; }
        public string RelationName { get; }
        public string SchemaName { get; }
        public InsertReturns Returns { get; }
    }
}