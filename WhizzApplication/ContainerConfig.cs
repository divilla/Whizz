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
            builder.AddWhizzOrm<ApplicationDbContext>(c =>
            {
                c.Connection = new NpgsqlConnection("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3");
                c.Context = new ApplicationDbContext(c.Connection);
                c.Assemblies.Add(Assembly.GetExecutingAssembly());
            });

            return builder.Build();
        }
    }
}