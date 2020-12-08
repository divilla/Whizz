using Npgsql;
using WhizzORM.Context;

namespace WhizzApplication
{
    public class ApplicationRepository : Repository
    {
        public ApplicationRepository(NpgsqlConnection connection) : base(connection) 
        {}
    }
}