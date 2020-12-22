using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Interfaces;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;

namespace WhizzJsonRepository.Base
{
    public abstract class Validator
    {
        public static T Validate<T>(JObject jsonRequest, JsonResponse response, IDatabase database, string relationName, string schemaName = IDbSchema.DefaultSchema)
            where T : Validator, new()
        {
            var validator = new T
            {
                JsonRequest = jsonRequest, 
                DbRequest = new JObject(),
                DbToJsonName = new Dictionary<string, string>(),
                Response = response,
                Database = database,
                RelationName = relationName,
                SchemaName = schemaName,
            };

            validator.Validate();

            return validator;
        }

        public T Next<T>() where T : Validator, new()
        {
            if (!Response.Continue) return null;
            
            var validator = new T
            {
                JsonRequest = JsonRequest,
                DbRequest = DbRequest,
                DbToJsonName = DbToJsonName,
                Response = Response,
                Database = Database,
                RelationName = RelationName,
                SchemaName = SchemaName,
            };

            validator.Validate();

            return validator;
        }

        protected JObject JsonRequest;
        protected JObject DbRequest;
        protected Dictionary<string, string> DbToJsonName;
        protected JsonResponse Response;
        protected IDatabase Database;
        protected string RelationName;
        protected string SchemaName;

        protected ImmutableArray<ColumnEntity> Columns => Database.Schema.GetColumns(RelationName, SchemaName);
        protected ImmutableArray<string> ColumnNames => Database.Schema.GetColumnNames(RelationName, SchemaName);

        protected abstract void Validate();
    }
}