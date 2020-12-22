using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using WhizzJsonRepository.Base;
using WhizzJsonRepository.Interfaces;
using WhizzJsonRepository.Utilities;
using WhizzJsonRepository.Validators;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Repository
{
    public abstract class JsonRepository
    {
        public JsonRepository(IConnectionFactory connectionFactory)
        {
            Database = connectionFactory.Database;
            Connection = connectionFactory.Connection;
        }

        protected IDatabase Database;
        protected NpgsqlConnection Connection;
        protected abstract RelationSchema ListRelationSchema { get; }
        protected abstract RelationSchema DetailRelationSchema { get; }
        protected abstract RelationSchema EntityRelationSchema { get; }

        public QueryArrayResult FindAllList(JObject filter = null)
        {
            return new Query(Database, Connection, ListRelationSchema).All();
        }
        
        public async Task<QueryArrayResult> FindAllListAsync()
        {
            return await new Query(Database, Connection, ListRelationSchema).AllAsync();
        }
        
        public QueryObjectResult FindByPrimaryKey(JObject request)
        {
            var response = new QueryObjectResult();
            Validator.Validate<GenericPropertiesValidator>(request, response, Database, ListRelationSchema.RelationName, ListRelationSchema.SchemaName)
                .Next<GenericPrimaryKeyValidator>();

            if (!response.Success)
            {
                return response;
            }
            
            return new Query(Database, Connection, ListRelationSchema)
                .Where(request)
                .One();
        }
    }
}
