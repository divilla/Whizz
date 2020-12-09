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
    public class FindJsonInvoker
    {
        public FindJsonInvoker(JsonRepository repository, string relationName, string schemaName)
        {
            _repository = repository;
            _relationName = relationName;
            _schemaName = schemaName;
        }

        private JsonRepository _repository;
        private string _relationName;
        private string _schemaName;

        public JsonResponseState One(JObject primaryKey)
        {
            var response = new JsonResponseState(primaryKey);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => q.IsPrimaryKey).ToImmutableArray();
            BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.All, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>()
                .Next<FindOneJsonRequestHandler>();

            return response;
        }

        public async Task<JsonResponseState> OneAsync(JObject primaryKey)
        {
            var response = new JsonResponseState(primaryKey);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => q.IsPrimaryKey).ToImmutableArray();
            var handler = BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.All, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>();
            await handler.NextAsync<FindOneJsonRequestHandler>();

            return response;
        }

        public JsonResponseState All(JObject where = default)
        {
            var response = new JsonResponseState(where);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName);
            BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>()
                .Next<FindAllJsonRequestHandler>();

            return response;
        }

        public async Task<JsonResponseState> AllAsync(JObject where = null)
        {
            where ??= new JObject();
            var response = new JsonResponseState(where);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName);
            var handler = BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>();
            await handler.NextAsync<FindAllJsonRequestHandler>();

            return response;
        }
    }
}