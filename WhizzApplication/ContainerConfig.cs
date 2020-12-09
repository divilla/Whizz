using System.Reflection;
using Autofac;
using Npgsql;
using WhizzJsonRepository;
using WhizzJsonRepository.Repository;

namespace WhizzApplication
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            var connection = new NpgsqlConnection("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3");
            builder.AddWhizzJsonRepository(c =>
            {
                c.Connection = connection;
                c.Repository = new JsonRepository(connection);
                c.Assemblies.Add(Assembly.GetExecutingAssembly());
            });

            return builder.Build();
        }
    }
}