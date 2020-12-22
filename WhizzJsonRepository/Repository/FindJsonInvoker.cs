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
        public FindJsonInvoker(IJsonRepository repository)
        {
            _repository = repository;
        }

        private IJsonRepository _repository;

        public JsonResponse One(JObject filter)
        {
            var response = new JsonResponse();
            
            BaseJsonHandler
                .Init(ref response, _repository)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidator>()
                .Next<FindOneJsonRequestHandler>();

            return response;
        }

        // public async Task<ResponseState> OneAsync(JObject primaryKey)
        // {
        //     var response = new ResponseState(primaryKey);
        //     var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => q.IsPrimaryKey).ToImmutableArray();
        //     var handler = BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.All, _relationName, _schemaName)
        //         .Next<GenericRequiredJsonValidatorHandler>()
        //         .Next<GenericTypeJsonValidatorHandler>();
        //     await handler.NextAsync<FindOneJsonRequestHandler>();
        //
        //     return response;
        // }
        //
        // public ResponseState All(JObject where = default)
        // {
        //     var response = new ResponseState(where);
        //     var columns = _repository.Schema.GetColumns(_relationName, _schemaName);
        //     BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
        //         .Next<GenericRequiredJsonValidatorHandler>()
        //         .Next<GenericTypeJsonValidatorHandler>()
        //         .Next<FindAllJsonRequestHandler>();
        //
        //     return response;
        // }
        //
        // public async Task<ResponseState> AllAsync(JObject where = null)
        // {
        //     where ??= new JObject();
        //     var response = new ResponseState(where);
        //     var columns = _repository.Schema.GetColumns(_relationName, _schemaName);
        //     var handler = BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.None, _relationName, _schemaName)
        //         .Next<GenericRequiredJsonValidatorHandler>()
        //         .Next<GenericTypeJsonValidatorHandler>();
        //     await handler.NextAsync<FindAllJsonRequestHandler>();
        //
        //     return response;
        // }
    }
}