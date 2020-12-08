using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzORM.Interfaces;
using WhizzORM.JsonValidate;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class JsonRepository
    {
        public JsonRepository(NpgsqlConnection connection, IJsonTypeValidator typeValidator = null)
        {
            Connection = connection;
            Schema = new DbSchema(connection.ConnectionString);
            TypeValidator = typeValidator ?? new JsonTypeValidator();
            Connection.OpenAsync().Wait();
        }

        public NpgsqlConnection Connection { get; }
        public IDbSchema Schema { get; }
        public IJsonTypeValidator TypeValidator { get; }
        public Func<string, string> Quote => Schema.Quote;
        public virtual Case DbCase => Case.Snake;
        public virtual Func<string, string> DbCaseResolver => (s) => s.ToSnakeCase(); 
        public virtual Func<string, string> DbQuotedCaseResolver => (s) => Quote(s.ToSnakeCase()); 
        public virtual Case JsonCase => Case.Camel;
        public virtual Func<string, string> JsonCaseResolver => (s) => s.ToCamelCase();
        public virtual Func<string, string> JsonQuotedCaseResolver => (s) => Quote(s.ToCamelCase()); 

        public async Task<string> FindAllAsync(JObject whereData, string relationName, string schemaName = DbSchema.DefaultSchema)
        {
            var result = await _findAsync("json_agg", whereData, relationName, schemaName);

            return result == null ? "[]" : result.ToString();
        }
        
        public async Task<string> FindOneAsync(JObject whereData, string relationName, string schemaName = DbSchema.DefaultSchema)
        {
            var result = await _findAsync("row_to_json", whereData, relationName, schemaName);
            return result?.ToString();
        }

        public async Task<string> InsertAsync(JObject data, string relationName, string schemaName = DbSchema.DefaultSchema)
        {
            return "";
        }

        private async Task<object> _findAsync(string func, JObject whereData, string relationName, string schemaName = DbSchema.DefaultSchema)
        {
            var command = new NpgsqlCommand {Connection = Connection};
            var from = Schema.QuotedRelationName(relationName, schemaName);
            var columnNames = Schema.GetColumnNames(relationName, schemaName);
            var select = string.Join(", ", columnNames.Select(s => $"s.{Quote(s)} AS {JsonQuotedCaseResolver(s)}"));
            var resolvedWhereData = whereData.ResolvePropertyNames(DbCaseResolver);
            if (resolvedWhereData.Properties().Any())
            {
                var where = string.Join(" AND ", columnNames
                    .Where(q => resolvedWhereData.ContainsKey(q))
                    .Select(s => $"s.{Quote(s)} = p.{Quote(s)}"));
                var sql = $"SELECT {func}(t) FROM (SELECT {select} FROM {from} s, json_populate_record(null::{from}, @json) p WHERE {where}) t";
                command.CommandText = sql;
                command.Parameters.Add("json", NpgsqlDbType.Json);
                command.Parameters[0].Value = resolvedWhereData.ToString(Formatting.None);
            }
            else
            {
                var sql = $"SELECT {func}(t) FROM (SELECT {select} FROM {from} s) t";
                command.CommandText = sql;
            }

            return await command.ExecuteScalarAsync();
        }
    }
}
