using System;
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Repository;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Base
{
    public abstract class BaseJsonHandler
    {
        public BaseJsonHandler(JsonRepository repository, string request)
        {
            Repository = repository;
            Response = new JsonResponse();
            Deserialize(request);
        }

        protected JsonRepository Repository;
        protected JObject Data;
        public JsonResponse Response;
        protected ImmutableArray<ColumnEntity> Columns;

        protected void Deserialize(string request)
        {
            try
            {
                Data = JObject.Parse(request);
            }
            catch (Exception e)
            {
                Response.SetBadRequestError($"Unable to deserialize request. Exception: '{e.Message}'");
                Response.Continue = false;
            }
        }
    }
}
