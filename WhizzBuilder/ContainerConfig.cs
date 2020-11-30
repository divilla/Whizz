using Autofac;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzBuilder
{
    public static class ContainerConfig
    {
        public static IContainer Configure(Config config)
        {
            var builder = new ContainerBuilder();
            builder.Register(e => config);
            builder.Register<IDbSchema>(e => new DbSchema(config.ConnectionString));
            builder.RegisterType<Application>().AsSelf();
            return builder.Build();
        }
    }
}