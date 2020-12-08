using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzORM.Context;
using WhizzSchema.Interfaces;

namespace WhizzORM.JsonValidate
{
    public class JsonQueryCommandInvoker
    {
        public JsonQueryCommandInvoker(JsonRepository repository)
        {
            _repository = repository;
        }

        private readonly JsonRepository _repository;

        public JsonResponseState ValidatePrimaryKey(JObject data, string relationName, string schemaName = IDbSchema.DefaultSchema, List<string> primaryKeys = null)
        {
            var columns = primaryKeys == null 
                ? _repository.Schema.GetColumns(relationName, schemaName).Where(q => q.IsPrimaryKey).ToImmutableArray()
                : _repository.Schema.GetColumns(relationName, schemaName).Where(q => primaryKeys.Contains(q.ColumnName)).ToImmutableArray();
            
            var state = new JsonResponseState(data, columns, MandatoryColumns.All);

            BaseJsonHandler
                .Init(_repository, ref state)
                .Next<GenericJsonRequiredValidatorHandler>()
                .Next<GenericJsonTypeValidatorHandler>();

            return state;
        }
    }
}