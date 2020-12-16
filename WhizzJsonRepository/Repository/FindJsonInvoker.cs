using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;
using WhizzJsonRepository.Handlers;
using WhizzJsonRepository.Interfaces;
using WhizzJsonRepository.Validators;
// ReSharper disable MethodHasAsyncOverload

namespace WhizzJsonRepository.Repository
{
    public class FindJsonInvoker
    {
        public FindJsonInvoker(IRepository repository)
        {
            _repository = repository;
        }

        private IRepository _repository;

        public JsonResponseState One(JObject primaryKey)
        {
            var response = new JsonResponseState(primaryKey);
            var columns = _repository.Db.Schema
                .GetColumns(_repository.RelationName, _repository.SchemaName)
                .Where(q => q.IsPrimaryKey).ToImmutableArray();
            
            QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.All)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>()
                .Next<FindOneJsonRequestHandler>();

            return response;
        }

        public async Task<JsonResponseState> OneAsync(JObject primaryKey)
        {
            var response = new JsonResponseState(primaryKey);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => q.IsPrimaryKey).ToImmutableArray();
            var handler = QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.All, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>();
            await handler.NextAsync<FindOneJsonRequestHandler>();

            return response;
        }

        public JsonResponseState All(JObject where = default)
        {
            var response = new JsonResponseState(where);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName);
            QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
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
            var handler = QueryJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidatorHandler>();
            await handler.NextAsync<FindAllJsonRequestHandler>();

            return response;
        }
    }
}