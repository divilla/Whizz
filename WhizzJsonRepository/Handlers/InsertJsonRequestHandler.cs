using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Handlers
{
    public class InsertRequestHandler : QueryJsonHandler
    {
        protected override QueryJsonHandler Handle()
        {
            var command = InitCommand();
            
            try
            {
                var result = command.ExecuteScalar();
                if (result == null)
                    State.SetNotFoundError();

                State.SetData(result?.ToString());
            }
            catch (Exception)
            {
                State.SetData(State.OriginalRequest.ToString());
                State.SetBadRequestError();
            }

            return this;
        }

        protected override async Task<QueryJsonHandler> HandleAsync()
        {
            var command = InitCommand();
            
            try
            {
                var result = await command.ExecuteScalarAsync();
                if (result == null)
                    State.SetNotFoundError();

                State.SetData(result?.ToString());
            }
            catch (Exception)
            {
                State.SetData(State.OriginalRequest.ToString());
                State.SetBadRequestError();
            }

            return this;
        }

        private NpgsqlCommand InitCommand()
        {
            var select = string.Join(", ", State.Request.Properties().Select(s => DbQuote(s.Name)));
            var into = QuotedRelationName;
            var returning = string.Join(", ", Repository.Schema.GetPrimaryKeyColumnNames(RelationName, SchemaName)
                .Select(s => $"'{ToJsonCase(s)}', {DbQuote(s)}"));
            var sql = $"INSERT INTO {into} ({select}) SELECT {select} FROM json_populate_record(null::{into}, @json) RETURNING json_build_object({returning});";
            var command = new NpgsqlCommand(sql, Repository.Connection);
            command.Parameters.Add("json", NpgsqlDbType.Json);
            command.Parameters[0].Value = State.Request.ToString(Formatting.None);

            return command;
        }
    }
}