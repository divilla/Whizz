using Microsoft.Extensions.DependencyInjection;
using WhizzSchema.Interfaces;

namespace WhizzSchema
{
    public static class WhizzSchemaStartup
    {
        public static void AddWhizzSchema(this IServiceCollection services, string connectionString) {
            services.AddSingleton<IDbSchema>(o => new DbSchema(connectionString));
        }
    }
}
