using System;
using System.Collections.Concurrent;
using WhizzORM.Base;
using WhizzORM.Interfaces;
using WhizzORM.Schema;

namespace WhizzORM.Context
{
    public class DbContext : IDbContext
    {
        private readonly IDbSchema _dbSchema;
        private readonly ConcurrentDictionary<Type, EntityMap> _entityMaps = new ConcurrentDictionary<Type, EntityMap>();

        public DbContext(IDbSchema dbSchema, Action<IDbContext> configure)
        {
            _dbSchema = dbSchema;
            configure(this);
        }

        public EntityMapBuilder<TEntity> Set<TEntity>() where TEntity : class, new()
        {
            return new EntityMapBuilder<TEntity>(this, _dbSchema);
        }
        
        internal void Map(EntityMap entityMap)
        {
            if (_entityMaps.ContainsKey(entityMap.Type))
                throw new ArgumentException($"'{entityMap.AssemblyQualifiedName}' already mapped in '{GetType().AssemblyQualifiedName}'");

            _entityMaps[entityMap.Type] = entityMap;
        }
    }
}
