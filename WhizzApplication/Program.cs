using Autofac;

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