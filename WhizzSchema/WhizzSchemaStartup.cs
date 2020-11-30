using Autofac;
using WhizzSchema.Interfaces;

namespace WhizzSchema
{
    public static class WhizzSchemaStartup
    {
        public static void AddWhizzSchema(this ContainerBuilder builder, string connectionString) {
            builder.Register<IDbSchema>(o => new DbSchema(connectionString));
        }
    }
}
