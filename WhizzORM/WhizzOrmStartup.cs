using System.Reflection;
using Autofac;
using MediatR;

namespace WhizzORM
{
    public static class WhizzOrmStartup
    {
        public static void AddWhizzOrm(this ContainerBuilder builder, params Assembly[] assemblies) {
            var openHandlersTypes = new[] { typeof(IRequestHandler<,>), typeof(INotificationHandler<>) };
            foreach (var openHandlerType in openHandlersTypes)
            {
                builder
                    .RegisterAssemblyTypes(assemblies)
                    .AsClosedTypesOf(openHandlerType)
                    .InstancePerDependency();
            }        
        }
    }
}
