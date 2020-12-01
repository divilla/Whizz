using System.Reflection;
using Autofac;
using Npgsql;
using WhizzORM;

namespace WhizzApplication
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.AddWhizzOrm(Assembly.GetAssembly(typeof(WhizzOrmStartup)), Assembly.GetExecutingAssembly());
            var connection = new NpgsqlConnection("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3");
            builder.Register(s => connection);
            builder.Register(s => new ApplicationDbContext(connection));

            return builder.Build();
        }
    }
}