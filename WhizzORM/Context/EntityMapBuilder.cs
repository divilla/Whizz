using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WhizzORM.Base;
using WhizzORM.Helpers;
using WhizzORM.Interfaces;

namespace WhizzORM.Context
{
    public class EntityMapBuilder<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _dbContext;
        private readonly IDbSchema _dbSchema;
        private readonly EntityMap _entityMap;

        internal EntityMapBuilder(DbContext dbContext, IDbSchema dbSchema)
        {
            _dbContext = dbContext;
            _dbSchema = dbSchema;

            var type = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(type.AssemblyQualifiedName)) throw new Exception($"{type.FullName} is not well defined class with properties.");
            
            _entityMap = new EntityMap(type);
        }

        public EntityMapBuilder<TEntity> SetSchemaName(string schemaName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                throw new ArgumentNullException(nameof(schemaName), $"Schema name can't be set empty for Type '{_entityMap.FullName}");
            
            _entityMap.SchemaName = schemaName;
            
            return this;
        }

        public EntityMapBuilder<TEntity> SetRelationName(string relationName)
        {
            if (string.IsNullOrWhiteSpace(relationName))
                throw new ArgumentNullException(nameof(relationName), $"Relation name can't be set empty on binding Type '{_entityMap.FullName}.");
            
            _entityMap.RelationName = relationName;
            
            return this;
        }

        public EntityMapBuilder<TEntity> BindColumnName<TMember>(Expression<Func<TEntity, TMember>> expression, string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName), $"Column name can't be set empty on binding Type '{_entityMap.FullName}.");

            if (!(expression.Body is MemberExpression member && member.Member is PropertyInfo propertyInfo))
                throw new ArgumentException($"Expression does not return property of {_entityMap.FullName}", nameof(expression));

            _entityMap.MapColumnNameToProperty(columnName, propertyInfo);

            return this;
        }

        public void Map()
        {
            _mapSchema();
            _mapRelation();
            _mapColumns();
            _checkForMissingColumns();
            _checkForInvalidTypes();
            
            _dbContext.Map(_entityMap);
        }

        private void _mapSchema()
        {
            if (!_dbSchema.SchemaExists(_entityMap.SchemaName))
                throw new WhizzException($"Schema '{_entityMap.SchemaName}' does not exist in database '{_dbSchema.DatabaseName}' on binding Type '{_entityMap.FullName}'.");
        }

        private void _mapRelation()
        {
            if (!string.IsNullOrWhiteSpace(_entityMap.RelationName))
            {
                if (!_dbSchema.RelationExists(_entityMap.RelationName, _entityMap.SchemaName))
                    throw new WhizzException($"Relation '{_entityMap.RelationName}' does not exist in schema '{_entityMap.SchemaName}' in database '{_dbSchema.DatabaseName}' on binding Type '{_entityMap.FullName}'.");

                _entityMap.Schema = _dbSchema.GetRelationSchema(_entityMap.RelationName, _entityMap.SchemaName);
                return;
            }
            
            var typeName = _entityMap.Type.Name;
            var names = new[] {typeName, typeName.ToSnakeCase(), typeName.ToCamelCase()};
            foreach (var name in names)
            {
                if (!_dbSchema.RelationExists(name, _entityMap.SchemaName)) continue;

                _entityMap.RelationName = name;
                _entityMap.Schema = _dbSchema.GetRelationSchema(name, _entityMap.SchemaName);
                return;
            }
            
            throw new WhizzException($"Relation '{names[0]}' or '{names[1]}' or '{names[2]}' does not exist in schema '{_entityMap.SchemaName}' in database '{_dbSchema.DatabaseName}' on binding Type '{_entityMap.FullName}'.");
        }

        private void _mapColumns()
        {
            var properties = _entityMap.Type.GetProperties();
            if (properties.Length == 0)
                throw new WhizzException("Entity class doesn't have any public properties.");
            
            foreach (var propertyInfo in properties)
            {
                if (_entityMap.HasPropertyMapped(propertyInfo)) continue;
                
                var propertyName = propertyInfo.Name;
                var names = new[] {propertyName, propertyName.ToSnakeCase(), propertyName.ToCamelCase()};
                foreach (var name in names)
                {
                    if (!_entityMap.Schema.ColumnNames.Contains(name)) continue;
            
                    _entityMap.MapColumnNameToProperty(name, propertyInfo);
                    break;
                }
            }
        }

        private void _checkForMissingColumns()
        {
            var missingColumns = _entityMap.Schema.ColumnNames.Where(columnName => _entityMap.GetPropertyMap(columnName) == null).ToArray();
            if (!missingColumns.Any()) return;
            
            var missingColumnsQuoted = missingColumns.Select(s => $"'s'").ToArray();
            throw new WhizzException($"Columns: {string.Join(", ", missingColumnsQuoted)} are not bound to Type '{_entityMap.FullName}'. Please add properties binded to missing columns.");
        }

        private void _checkForInvalidTypes()
        {
            foreach (var column in _entityMap.Schema.Columns.Values)
            {
                var propertyMap = _entityMap.GetPropertyMap(column.ColumnName);
                if (propertyMap == null)
                    throw new WhizzException($"Column: '{column.ColumnName}' is not bound to Type '{_entityMap}'. Please add missing property bound to column.");

                if (column.ClrTypes.Contains(propertyMap.PropertyInfo.PropertyType)) continue;
                
                var typesStr = column.ClrTypes.Select(s => $"'{s.Name}'");
                throw new WhizzException($"Column of data type {column.DataType} of Type '{_entityMap.FullName}' is of invalid type. Please change type to one of: {string.Join(", ", typesStr)}.");
            }
        }
    }
}
