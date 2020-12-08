using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MediatR;
using Npgsql;
using WhizzORM.Context;
using WhizzORM.Interfaces;

namespace WhizzORM
{
    public static class WhizzOrmStartup
    {
        public static void AddWhizzOrm(this ContainerBuilder builder, Action<WhizzOrmStartupConfiguration> configuration)
        {
            var config = new WhizzOrmStartupConfiguration();
            configuration(config);

            var thisAssembly = Assembly.GetAssembly(typeof(WhizzOrmStartup));
            if (!config.Assemblies.Contains(thisAssembly))
                config.Assemblies.Add(thisAssembly);

            builder.Register(c => config.Connection);

            builder.Register(c => config.Repository).AsSelf().As<IRepository>();

            builder.Register(c => config.JsonRepository).AsSelf();
            
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var openHandlersTypes = new[] { typeof(IRequestHandler<,>), typeof(INotificationHandler<>) };
            foreach (var openHandlerType in openHandlersTypes)
            {
                builder
                    .RegisterAssemblyTypes(config.Assemblies.ToArray())
                    .AsClosedTypesOf(openHandlerType)
                    .InstancePerDependency();
            }        
        }
    }
}
