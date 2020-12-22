using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using WhizzBase.Enums;
using WhizzJsonRepository.Interfaces;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Utilities
{
    public class Query
    {
        public Query(IDatabase database, NpgsqlConnection connection, RelationSchema relationSchema)
        {
            _db = database;
            _connection = connection;
            _relationSchema = relationSchema;
        }

        private readonly IDatabase _db;
        private readonly NpgsqlConnection _connection;
        private readonly RelationSchema _relationSchema;

        private static string ResultPrefix => "r";
        private static string SourcePrefix => "s";
        private static string FilterPrefix => "f";

        private JObject _where;
        private readonly Dictionary<string, OrderByDirection> _orderBy = new Dictionary<string, OrderByDirection>();

        public Query Select(params string[] columns)
        {
            return this;
        }

        public Query Where(JObject where)
        {
            _where = where;
            return this;
        }

        public Query OrderBy(string column, OrderByDirection direction = OrderByDirection.Asc)
        {
            _orderBy[column] = direction;
            return this;
        }
        
        public Query ThenBy(string column, OrderByDirection direction = OrderByDirection.Asc)
        {
            _orderBy[column] = direction;
            return this;
        }

        public QueryObjectResult One()
        {
            try
            {
                string result;
                using (var command = new NpgsqlCommand(OneSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = command.ExecuteScalar().ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public async Task<QueryObjectResult> OneAsync()
        {
            try
            {
                string result;
                await using (var command = new NpgsqlCommand(OneSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = (await command.ExecuteScalarAsync()).ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public QueryObjectResult Exists()
        {
            try
            {
                string result;
                using (var command = new NpgsqlCommand(ExistsSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = command.ExecuteScalar().ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public async Task<QueryObjectResult> ExistsAsync()
        {
            try
            {
                string result;
                await using (var command = new NpgsqlCommand(ExistsSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = (await command.ExecuteScalarAsync()).ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public QueryObjectResult Count()
        {
            try
            {
                string result;
                using (var command = new NpgsqlCommand(CountSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = command.ExecuteScalar().ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public async Task<QueryObjectResult> CountAsync()
        {
            try
            {
                string result;
                await using (var command = new NpgsqlCommand(CountSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = (await command.ExecuteScalarAsync()).ToString();
                }

                return new QueryObjectResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryObjectResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public QueryArrayResult All()
        {
            try
            {
                string result;
                using (var command = new NpgsqlCommand(AllSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = command.ExecuteScalar().ToString();
                }

                return new QueryArrayResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryArrayResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        public async Task<QueryArrayResult> AllAsync()
        {
            try
            {
                string result;
                await using (var command = new NpgsqlCommand(AllSql(), _connection))
                {
                    if (command.CommandText.Contains("@json"))
                    {
                        command.Parameters.Add("json", NpgsqlDbType.Json);
                        command.Parameters[0].Value = _where.ToString();
                    }
                    result = (await command.ExecuteScalarAsync()).ToString();
                }

                return new QueryArrayResult(result);
            }
            catch (Exception e)
            {
                var result = new QueryArrayResult();
                result.SetDatabaseException(e.Message);
                return result;
            }
        }

        private string OneSql()
        {
            return _where.Properties().Any() 
                ? $"SELECT row_to_json({ResultPrefix}) FROM ({BuildSelectSource()}, {BuildFilter()} WHERE {BuildFind()}) {ResultPrefix};" 
                : $"SELECT row_to_json({ResultPrefix}) FROM ({BuildSelectSource()}) {ResultPrefix};";
        }
        
        private string AllSql()
        {
            return _where.Properties().Any() 
                ? $"SELECT json_agg({ResultPrefix}) FROM ({BuildSelectSource()}, {BuildFilter()} WHERE {BuildFind()}) {ResultPrefix};" 
                : $"SELECT json_agg({ResultPrefix}) FROM ({BuildSelectSource()}) {ResultPrefix};";
        }

        private string ExistsSql()
        {
            return _where.Properties().Any() 
                ? $"SELECT json_build_object('exists', EXISTS(SELECT * FROM {_relationSchema.QuotedSchemaRelation}, {BuildFilter()} WHERE {BuildFind()}));" 
                : $"SELECT json_build_object('exists', EXISTS(SELECT * FROM {_relationSchema.QuotedSchemaRelation});";
        }
        
        private string CountSql()
        {
            return _where.Properties().Any() 
                ? $"SELECT json_build_object('exists', (SELECT COUNT(*) FROM {_relationSchema.QuotedSchemaRelation}, {BuildFilter()} WHERE {BuildFind()}));" 
                : $"SELECT json_build_object('exists', (SELECT COUNT(*) FROM {_relationSchema.QuotedSchemaRelation});";
        }
        
        private string BuildSelectSource()
        {
            var columnAliases = _db.Schema.SchemaRelations[_relationSchema.SchemaName][_relationSchema.RelationName].ColumnNames
                .Select(s => _db.ToQuotedDbCase(s) == _db.ToQuotedJsonCase(s) ? _db.ToQuotedDbCase(s) : $"{_db.ToQuotedDbCase(s)} AS {_db.ToQuotedJsonCase(s)}");
            var select = string.Join(", ", columnAliases);

            return _orderBy.Any() 
                ? $"(SELECT {select} FROM {_relationSchema.QuotedSchemaRelation} ORDER BY {BuildOrderBy()}) {SourcePrefix}" 
                : $"(SELECT {select} FROM {_relationSchema.QuotedSchemaRelation}) {SourcePrefix}";
        }
        
        private string BuildFilter()
        {
            return $", json_populate_record(null::{_relationSchema.QuotedSchemaRelation}, @json) {FilterPrefix}";
        }

        private string BuildFind()
        {
            var conditions = _where.Properties().Select(s => $"{SourcePrefix}.{s.Name} = {FilterPrefix}.{s.Name}");
            return string.Join(" AND ", conditions);
        }

        private string BuildOrderBy()
        {
            var orderByList = new List<string>();
            foreach (var (key, value) in _orderBy)
            {
                orderByList.Add(value == OrderByDirection.Desc ? $"{key} DESC" : key);
            }

            return string.Join(", ", orderByList);
        }
    }
}