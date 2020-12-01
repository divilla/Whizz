using System.Net.Mime;
using Autofac;
using Npgsql;
using Utf8Json;
using WhizzBase.Base;
using WhizzORM.Context;

namespace WhizzApplication
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();
            using var scope = container.BeginLifetimeScope();
            var dbContext = scope.Resolve<ApplicationDbContext>();
        }
    }
}