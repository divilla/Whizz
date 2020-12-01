using System;
using System.Collections.Immutable;
using WhizzBase.Helpers;
using WhizzSchema;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class RequestFactory<TEntity>
        where TEntity : class, new()
    {
        public RequestFactory(DbContext dbContext)
        {
            _requestType = typeof(TEntity);
            _dbContext = dbContext;
            _dbSchema = _dbContext.DbSchema;
            _resolveRelation();
            _columns = _dbSchema.GetColumns(_relation.RelationName, _relation.SchemaName);
            _foreignKeys = _dbSchema.GetForeignKeys(_relation.RelationName, _relation.SchemaName);
            _uniqueIndexes = _dbSchema.GetUniqueIndexes(_relation.RelationName, _relation.SchemaName);
        }

        private Type _requestType;
        private Type _responseType;
        private DbContext _dbContext;
        private IDbSchema _dbSchema;
        private RelationEntity _relation;
        private ImmutableArray<ColumnEntity> _columns;
        private ImmutableArray<ForeignKeyEntity> _foreignKeys;
        private ImmutableArray<UniqueIndexEntity> _uniqueIndexes;
        
        public RequestFactory<TEntity> GetAll()
        {
            return this;
        }

        public void Send()
        {
            
        }

        private void _resolveRelation()
        {
            var name = AttributeHelper.GetRelationSchemaName(_requestType);
            _relation = _dbSchema.UnquoteRelationName(name, _requestType);
        }
    }
}