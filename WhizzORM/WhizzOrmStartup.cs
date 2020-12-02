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
        public static void AddWhizzOrm<TDbContext>(this ContainerBuilder builder, Action<WhizzOrmStartupConfiguration<TDbContext>> configuration) 
            where TDbContext : DbContext<TDbContext>
        {
            var config = new WhizzOrmStartupConfiguration<TDbContext>();
            configuration(config);

            var thisAssembly = Assembly.GetAssembly(typeof(WhizzOrmStartup));
            if (!config.Assemblies.Contains(thisAssembly))
                config.Assemblies.Add(thisAssembly);

            builder.Register(c => config.Connection);

            builder.Register(c =>
            {
                var connection = c.Resolve<NpgsqlConnection>();
                return (TDbContext) Activator.CreateInstance(typeof(TDbContext), connection);
            }).AsSelf().As<IDbContext>();
            
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
