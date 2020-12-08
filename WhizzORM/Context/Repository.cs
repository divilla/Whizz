using System;
using Npgsql;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzORM.Interfaces;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class Repository : IRepository
    {
        public Repository(NpgsqlConnection connection)
        {
            Connection = connection;
            Schema = new DbSchema(connection.ConnectionString);
        }

        public NpgsqlConnection Connection { get; }
        public IDbSchema Schema { get; }
        public virtual Case DbCase => Case.Snake;
        public virtual Func<string, string> DbCaseResolver => (s) => s.ToSnakeCase(); 

        public InsertCommand<TData> Insert<TData>(string relationName, TData data)
        {
            return Insert(relationName, DbSchema.DefaultSchema, data);
        }

        public InsertCommand<TData> Insert<TData>(string relationName, string schemaName, TData data)
        {
            return new InsertCommand<TData>(this, data, relationName, schemaName);
        }


        // public ImmutableDictionary<Type, EntitySchema> EntitySchema { get; private set; }
        //
        // private void _init()
        // {
        //     var entitySchema = new Dictionary<Type, EntitySchema>();
        //     foreach (var propertyInfo in GetType().GetProperties())
        //     {
        //         var type = propertyInfo.PropertyType;
        //         if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(RequestFactory<>)) continue;
        //         
        //         var entityType = type.GetGenericArguments()[0];
        //         var instance = Activator.CreateInstance(propertyInfo.PropertyType, this);
        //         propertyInfo.SetValue(this, instance, null);
        //         
        //         var dbName = AttributeHelper.GetRelationDbName(entityType);
        //         var relation = DbSchema.UnquoteRelationName(dbName, entityType);
        //         var columns = DbSchema.GetColumns(relation.RelationName, relation.SchemaName);
        //
        //         var properties = new List<PropertySchema>();
        //         foreach (var property in entityType.GetProperties())
        //         {
        //             var columnEntity = _resolveColumnEntity(property, columns);
        //             if (columnEntity == null) continue;
        //             
        //             properties.Add(new PropertySchema(property.Name, property, columnEntity.ColumnName, columnEntity));
        //         }
        //         
        //         entitySchema[entityType] = new EntitySchema(entityType, dbName, relation.RelationName, relation.SchemaName, relation.IsReadOnly, properties.ToImmutableArray());
        //     }
        //
        //     EntitySchema = entitySchema.ToImmutableDictionary();
        // }
        //
        // private ColumnEntity _resolveColumnEntity(PropertyInfo property, ImmutableArray<ColumnEntity> columns)
        // {
        //     var columnAttribute = AttributeHelper.GetAttribute<ColumnAttribute>(property);
        //     if (columnAttribute != null)
        //     {
        //         var columnEntity = columns.SingleOrDefault(q => q.ColumnName == columnAttribute.Name);
        //         if (columnEntity != null) return columnEntity;
        //     }
        //
        //     var names = new[] {property.Name, property.Name.ToCamelCase(), property.Name.ToSnakeCase()};
        //     return columns.SingleOrDefault(q => names.Contains(q.ColumnName));
        // }
    }
}
