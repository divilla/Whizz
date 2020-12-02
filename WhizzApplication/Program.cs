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
        public static IContainer Container { get; private set; }
        public static void Main(string[] args)
        {
            Container = ContainerConfig.Configure();
        }
    }
}