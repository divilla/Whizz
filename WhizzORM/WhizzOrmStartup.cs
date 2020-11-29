using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WhizzORM.Context;
using WhizzORM.Interfaces;

namespace WhizzORM
{
    public static class WhizzOrmStartup
    {
        public static void AddWhizzOrm(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDbContext, DbContext>();
            services.AddScoped(config => new NpgsqlConnection(connectionString));
        }
    }
}
