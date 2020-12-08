using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using Utf8Json;
using Utf8Json.Resolvers;
using WhizzBase.Base;
using WhizzBase.Cache;
using WhizzBase.Enums;
using WhizzBase.Extensions;
using WhizzBase.Reflection;
using WhizzORM.Interfaces;
using WhizzSchema;

namespace WhizzORM.Context
{
    public class InsertCommand<TData> : RelationCommand<InsertCommand<TData>, TData>
    {
        public InsertCommand(IRepository repository, TData data, string relationName, string schemaName = DbSchema.DefaultSchema) : base(repository, data, relationName, schemaName)
        { }

        public async Task<T> ToNewTypeAsync<T>() where T : class, new()
        {
            var entity = new T();
            return await ToInstanceAsync<T>(entity);
        }

        public async Task<T> ToInstanceAsync<T>(T entity)
            where T : class, new()
        {
            var type = typeof(TData);
            await GetOrCreateCommand(type, RelationName, SchemaName, InsertReturns.Instance);
            BindParameters(type);

            var reader = await Command.ExecuteReaderAsync();
            var setters = ReflectionFactory.SetterDelegatesByPropertyName(typeof(T), Case.Snake);
            await reader.ReadAsync();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                setters[reader.GetName(i)](entity, reader.GetValue(i));
            }

            return entity;
        }

        public async Task<string> ToJsonAsync()
        {
            var type = typeof(TData);
            await GetOrCreateCommand(type, RelationName, SchemaName, InsertReturns.Json);
            BindParameters(type);
            
            return (await Command.ExecuteScalarAsync()).ToString();
        }

        private async Task GetOrCreateCommand(Type type, string relationName, string schemaName, InsertReturns returns)
        {
            var cacheEntry = new RelationCommandCacheKey(SqlCommandType.Insert, type, relationName, schemaName, returns);
            if (GlobalMemoryCache.ContainsKey(cacheEntry))
            {
                await SetCommand(GlobalMemoryCache.Get<RelationCommandCacheKey, NpgsqlCommand>(cacheEntry));
                return;
            }

            var resolver = Repository.DbCaseResolver;
            var primaryKeys = new List<string>();
            var columnNames = new List<string>();
            var propertyNames = ReflectionFactory.PropertiesInfo(type).Select(s => resolver(s.Name)).ToArray();
            foreach (var columnEntity in Repository.Schema.GetColumns(relationName, schemaName))
            {
                if (columnEntity.IsPrimaryKey)
                    primaryKeys.Add(columnEntity.ColumnName);
                
                if (!columnEntity.IsGenerated && propertyNames.Contains(columnEntity.ColumnName))
                {
                    columnNames.Add(columnEntity.ColumnName);
                }
            }

            var into = string.Join(", ", columnNames.Select(s => Repository.Schema.Quote(s)));
            var values = string.Join(", ", columnNames.Select(s => $"@{s}"));
            var returning = returns == InsertReturns.Json 
                ? $"json_build_object({string.Join(", ", primaryKeys.Select(s => $"'{s}', {s}"))})"
                : $"{string.Join(", ", primaryKeys)}";

            await SetSql($"INSERT INTO {QuotedRelationName} ({into}) VALUES ({values}) RETURNING {returning};");
            
            GlobalMemoryCache.AddOrUpdate(cacheEntry, Command);
        }
    }
}