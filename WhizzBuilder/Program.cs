using Autofac;
using Utf8Json;
using WhizzBase.Base;

namespace WhizzBuilder
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new DbException("Path to config.json must be specified as parameter at runtime.");
            
            if (!System.IO.File.Exists(args[0])) 
                throw new DbException("File config.json does not exist.");

            Config config;
            using (var configFileStream = System.IO.File.OpenRead(args[0]))
            {
                config = JsonSerializer.Deserialize<Config>(configFileStream);
            }

            config.TargetFolder = string.IsNullOrEmpty(config.TargetFolder)
                ? System.IO.Directory.GetCurrentDirectory()
                : config.TargetFolder;

            var container = ContainerConfig.Configure(config);
            using(var scope = container.BeginLifetimeScope())
            {
                var application = scope.Resolve<Application>();
                application.Build();
            }
        }
    }
}