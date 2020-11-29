using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using Npgsql;
using WhizzBase.Extensions;

namespace WhizzBase.Helpers
{
    public class DataReaderEntityMapper
    {
        public DataReaderEntityMapper(NpgsqlDataReader dataReader, Type entityType)
        {
            DataReader = dataReader;
            EntityType = entityType;
            Init();
        }

        public NpgsqlDataReader DataReader { get; }
        public Type EntityType { get; }
        public ImmutableDictionary<string, ColumnPropertyMapper> ColumnMaps { get; private set; }
        public ImmutableDictionary<PropertyInfo, ColumnPropertyMapper> PropertyMaps { get; private set; }
        
        private void Init()
        {
            var properties = new Dictionary<string, PropertyInfo>();
            foreach (var propertyInfo in EntityType.GetProperties())
                properties[propertyInfo.Name] = propertyInfo;
            
            var columnMaps = new Dictionary<string, ColumnPropertyMapper>();
            var propertyMaps = new Dictionary<PropertyInfo, ColumnPropertyMapper>();
            for (var i = 0; i < DataReader.FieldCount; i++)
            {
                var columnName = DataReader.GetName(i);
                if (properties.ContainsKey(columnName))
                {
                    columnMaps[columnName] = new ColumnPropertyMapper(columnName, properties[columnName]);
                    propertyMaps[properties[columnName]] = new ColumnPropertyMapper(columnName, properties[columnName]);
                    continue;
                }
                
                var pascalName = columnName.ToPascalCase();
                if (properties.ContainsKey(pascalName))
                {
                    columnMaps[columnName] = new ColumnPropertyMapper(columnName, properties[pascalName]);
                    propertyMaps[properties[pascalName]] = new ColumnPropertyMapper(columnName, properties[pascalName]);
                    continue;
                }
                
                var camelName = columnName.ToCamelCase();
                if (properties.ContainsKey(camelName))
                {
                    columnMaps[columnName] = new ColumnPropertyMapper(columnName, properties[camelName]);
                    propertyMaps[properties[camelName]] = new ColumnPropertyMapper(columnName, properties[camelName]);
                }
            }

            ColumnMaps = columnMaps.ToImmutableDictionary();
            PropertyMaps = propertyMaps.ToImmutableDictionary();
        }
    }
}
