using Autofac;
using WhizzSchema;

namespace WhizzBuilder
{
    public static class ContainerConfig
    {
        public static IContainer Configure(Config config)
        {
            var builder = new ContainerBuilder();
            builder.Register(e => config);
            builder.AddWhizzSchema(config.ConnectionString);
            builder.RegisterType<Application>().AsSelf();
            return builder.Build();
        }
    }
}