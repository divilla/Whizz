using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Npgsql;
using WhizzORM.Base;
using WhizzORM.Schema;

namespace WhizzORM.Helpers
{
    public class PgsqlRepository
    {
        private readonly DbSchema _schema;
        private readonly NpgsqlConnection _connection;

        public PgsqlRepository(DbSchema schema, NpgsqlConnection connection)
        {
            _schema = schema;
            _connection = connection;
            if (_connection.FullState != ConnectionState.Open)
                _connection.OpenAsync().Wait();
        }

        public async Task<string> Find(string relationName, params object[] primaryKeyValues)
        {
            return await Find(DbSchema.DefaultSchema, relationName, primaryKeyValues);
        }

        //public async Task<string> Find(string schemaName, string relationName, params dynamic[] primaryKeyValues)
        //{

        //}

        // public async Task<string> Insert<TPayload>(string tableName, TPayload payload, Guid userId, bool prepare = false)
        // {
        //     return await Insert<TPayload>(DbSchema.DEFAULT_SCHEMA, tableName, payload, userId, prepare);
        // }
        //
        // public async Task<string> Insert<TPayload>(string schemaName, string tableName, TPayload payload, Guid userId, bool prepare = false)
        // {
            //var quotedTableName = _schema.QuotedTableName(schemaName, tableName);
            //var columns = new ColumnDictionary(_schema);
            //columns.SetColumns(schemaName, tableName);
            //columns.SetValues<TPayload>(payload);
            //var primaryKeyColumnNames = _schema.PrimaryKeyColumnNames(schemaName, tableName);
            //var isPrimaryKeyGenerated = _schema.PrimaryKeyIsGenerated(schemaName, tableName);

            //var commandStatement = $"INSERT INTO {quotedTableName}"; 
            //commandStatement += $" ({string.Join(", ", columns.Keys.Select(s => _schema.Quote(s)).ToArray())})";
            //commandStatement += $" VALUES ({string.Join(", ", columns.Keys.Select(s => $"@{s}").ToArray())})";
            //commandStatement += isPrimaryKeyGenerated ? $" RETURNING {primaryKeyColumnNames[0]};" : $";";

            //var command = new NpgsqlCommand(commandStatement, _connection);
            //NpgsqlCommandBuilder.DeriveParameters(command);
            //foreach (var columnValue in columns.Values)
            //    command.Parameters[columnValue.ColumnEntity.ColumnName].Value = columnValue.Value;
            
            //if (prepare) await command.PrepareAsync();

            //return "";

            //var jsonPairs = _schema.Columns[schemaName][detailViewName].Keys.Select(columnName => $"'{columnName.ToCamelCase()}', {columnName}").ToList();
            //var selectStatement =
            //    $"SELECT json_build_object({string.Join(", ", jsonPairs)}) FROM {schemaName}.{detailViewName}";
            //object primaryKeyValue = null;
            //if (isPrimaryKeyGenerated)
            //{
            //    primaryKeyValue = await command.ExecuteScalarAsync();
            //    selectStatement = $"{selectStatement} WHERE {primaryKeyColumnNames[0]} = @{primaryKeyColumnNames[0]};";
            //}
            //else
            //{
            //    await command.ExecuteNonQueryAsync();
            //    var conditions = primaryKeyColumnNames.Select(s => $"{s} = @{s}").ToList();
            //    selectStatement = $"{selectStatement} WHERE {string.Join(" AND  ", conditions)};";
            //}

            //var selectCommand = new NpgsqlCommand(selectStatement, _connection);
            //NpgsqlCommandBuilder.DeriveParameters(selectCommand);
            //if (isPrimaryKeyGenerated)
            //    selectCommand.Parameters[0].Value = primaryKeyValue;
            //else
            //    foreach (var columnName in primaryKeyColumnNames)
            //        selectCommand.Parameters[columnName].Value = payloadDict[columnName];

            //return (await selectCommand.ExecuteScalarAsync()).ToString();
        // }
    }
}
