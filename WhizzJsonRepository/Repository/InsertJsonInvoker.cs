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
        public InsertJsonInvoker(TableJsonRepository repository, string relationName, string schemaName)
        {
            _repository = repository;
            _relationName = relationName;
            _schemaName = schemaName;
        }

        private TableJsonRepository _repository;
        private string _relationName;
        private string _schemaName;

        public JsonResponse Values(JObject values)
        {
            var response = new JsonResponse(values);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => !q.IsReadonly).ToImmutableArray();
            BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.Required, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidator>()
                .Next<InsertRequestHandler>();

            return response;
        }

        public async Task<JsonResponse> ValuesAsync(JObject values)
        {
            var response = new JsonResponse(values);
            var columns = _repository.Schema.GetColumns(_relationName, _schemaName).Where(q => !q.IsReadonly).ToImmutableArray();
            var handler = BaseJsonHandler.Init(ref response, _repository, columns, MandatoryColumns.Required, _relationName, _schemaName)
                .Next<GenericRequiredJsonValidatorHandler>()
                .Next<GenericTypeJsonValidator>();
            await handler.NextAsync<InsertRequestHandler>();

            return response;
        }
    }
}