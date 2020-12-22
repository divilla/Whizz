using System.Threading.Tasks;
using WhizzJsonRepository.Base;
using WhizzJsonRepository.Repository;
using WhizzJsonRepository.Validators;

namespace WhizzJsonRepository.Handlers
{
    public class FindByPrimaryKeyHandler : BaseJsonHandler
    {
        public FindByPrimaryKeyHandler(JsonRepository repository, string request) : base(repository, request)
        {
            if (!Response.Continue) return;

            Columns = repository.PrimaryKeyColumnSchema;
            Validator.Validate<GenericPropertiesValidator>(ref Data, ref Response, Columns);
            
            if (!Response.Continue) return;
        }

        public JsonResponse Query()
        {
            QueryAsync().Wait();

            return Response;
        }
        
        public async Task<JsonResponse> QueryAsync()
        {
            if (!Response.Success)
                return Response;
        }
    }
}