using System.Reflection;
using Autofac;
using Npgsql;
using WhizzORM;
using WhizzORM.Context;

namespace WhizzApplication
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            var connection = new NpgsqlConnection("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3");
            builder.AddWhizzOrm(c =>
            {
                c.Connection = connection;
                c.Repository = new ApplicationRepository(connection);
                c.JsonRepository = new JsonRepository(connection);
                c.Assemblies.Add(Assembly.GetExecutingAssembly());
            });

            return builder.Build();
        }
    }
}