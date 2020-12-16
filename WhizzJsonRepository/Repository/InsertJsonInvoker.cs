using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;
using WhizzJsonRepository.Handlers;
using WhizzJsonRepository.Validators;
// ReSharper disable MethodHasAsyncOverload

namespace WhizzJsonRepository.Repository
{
    public class InsertJsonInvoker
    {
        public InsertJsonInvoker(JsonRepository<> repository, string relationName, string schemaName)
        {
            _repository = repository;
            _relationName = relationName;
            _schemaName = schemaName;
        }

        private JsonRepository<> _repository;
        private string _relationName;
        private string _schemaName;

        public JsonResponseState Values(JObject values)
        {
            var response = new JsonResponseState(values);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => !q.IsReadonly).ToImmutableArray();
            QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.Required, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>()
                .Next<InsertRequestHandler>();

            return response;
        }

        public async Task<JsonResponseState> ValuesAsync(JObject values)
        {
            var response = new JsonResponseState(values);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => !q.IsReadonly).ToImmutableArray();
            var handler = QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.Required, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>();
            await handler.NextAsync<InsertRequestHandler>();

            return response;
        }
    }
}