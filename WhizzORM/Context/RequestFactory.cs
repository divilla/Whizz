using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Npgsql;
using WhizzBase.Base;
using WhizzBase.Helpers;
using WhizzSchema;
using WhizzSchema.Entities;

namespace WhizzORM.Context
{
    public class RequestFactory<TEntity>
        where TEntity : class, new()
    {
        // public RequestFactory(NpgsqlConnection connection, DbSchema dbSchema)
        // {
        //     _connection = connection;
        //     _dbSchema = dbSchema;
        //     _type = typeof(TEntity);
        //     
        //     _resolveRelation();
        //     _columns = _dbSchema.GetColumns(_relation.RelationName, _relation.SchemaName);
        //     _foreignKeys = _dbSchema.GetForeignKeys(_relation.RelationName, _relation.SchemaName);
        //     _uniqueIndexes = _dbSchema.GetUniqueIndexes(_relation.RelationName, _relation.SchemaName);
        // }
        //
        // private NpgsqlConnection _connection;
        // private DbSchema _dbSchema;
        // private Type _type;
        // private RelationEntity _relation;
        // private ImmutableArray<ColumnEntity> _columns;
        // private ImmutableArray<ForeignKeyEntity> _foreignKeys;
        // private ImmutableArray<UniqueIndexEntity> _uniqueIndexes;
        
        private 
        public RequestFactory GetAll()
        {
            return this;
        }

        private void _resolveRelation()
        {
            var name = AttributeHelper.GetRelationSchemaName(_type);
            _relation = _dbSchema.UnquoteRelationName(name, _type);
        }
    }
}