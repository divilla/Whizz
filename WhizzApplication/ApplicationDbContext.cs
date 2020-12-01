using Domain;
using Npgsql;
using WhizzORM.Context;

namespace WhizzApplication
{
    public class ApplicationDbContext : DbContext
    {
        public RequestFactory<Test> Tests { get; protected set; }
        
        public ApplicationDbContext(NpgsqlConnection connection) : base(connection) 
        {}
    }
}