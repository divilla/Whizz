using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzORM
{
    public static class WhizzOrmStartup
    {
        public static void AddWhizzOrm(this IServiceCollection services, string connectionString)
        {
            services.AddScoped(config => new NpgsqlConnection(connectionString));
            services.AddSingleton<IDbSchema>(new DbSchema(connectionString));
        }
    }
}
