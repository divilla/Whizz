using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WhizzORM.Schema;

namespace WhizzORM.Context
{
    public enum RelationType
    {
        Table,
        PartitionedTable,
        View,
        MaterializedView
    }

    internal class EntityMap
    {
        internal bool CanReadFromDatabase { get; private set; }
        internal bool CanWriteToDatabase { get; private set; }
        internal Type Type { get; }
        internal string AssemblyQualifiedName { get; }
        internal string FullName { get; }
        internal string SchemaName { get; set; } = DbSchema.DefaultSchema;
        internal string RelationName { get; set; }
        internal RelationSchema Schema { get; set; }
        internal RelationType RelationType { get; set; }

        private readonly Dictionary<string, PropertyMap> _propertyMaps = new Dictionary<string, PropertyMap>();

        internal EntityMap(Type type)
        {
            CanReadFromDatabase = false;
            CanWriteToDatabase = false;
            Type = type;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            FullName = type.FullName;
        }
        
        internal void MapColumnNameToProperty(string columnName, PropertyInfo propertyInfo)
        {
            _propertyMaps[columnName] = new PropertyMap(columnName, propertyInfo);
        }

        internal PropertyMap GetPropertyMap(string columnName)
        {
            return _propertyMaps.ContainsKey(columnName) ? _propertyMaps[columnName] : null;
        }

        internal bool HasPropertyMapped(PropertyInfo propertyInfo)
        {
            return _propertyMaps.Values.Any(s => s.PropertyInfo == propertyInfo);
        }
    }
}
