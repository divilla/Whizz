using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Handlers
{
    public class FindOneJsonRequestHandler : BaseJsonHandler
    {
        protected override BaseJsonHandler Handle()
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

        protected override async Task<BaseJsonHandler> HandleAsync()
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
            var select = AllColumnNamesSelect("s");
            var from = QuotedRelationName;
            var where = string.Join(" AND ", State.Request.Properties()
                .Select(s => $"s.{DbQuote(s.Name)} = p.{DbQuote(s.Name)}"));
            var sql = $"SELECT row_to_json(t) FROM (SELECT {select} FROM {from} s, json_populate_record(null::{from}, @json) p WHERE {where}) t";
            var command = new NpgsqlCommand(sql, Repository.Connection);
            command.Parameters.Add("json", NpgsqlDbType.Json);
            command.Parameters[0].Value = State.Request.ToString(Formatting.None);

            return command;
        }
    }
}