using Autofac;

namespace WhizzApplication
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => 
                new QuitePosDb("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3;Minimum Pool Size=1"));
            builder.Register(c => 
                new QuitePosConnection("Host=localhost;Database=quitepos_demo;Username=quitepos;Password=Masa{}/3;Minimum Pool Size=1"));

            return builder.Build();
        }
    }
}