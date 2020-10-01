using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Builder;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SAEAbstractDependencyInjectionExtension
    {

        /// <summary>
        /// add <seealso cref="IMediator"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMediatorBuilder AddMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any()) assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            var mediatorHandler = typeof(IMediatorHandler);

            var descriptors = new List<CommandHandlerDescriptor>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetAssignableFrom(mediatorHandler)
                                             .Where(t => t.IsClass &&
                                                    !t.IsAbstract &&
                                                    !t.IsInterface))
                {

                    foreach (var interfaceType in type.GetInterfaces()
                                                      .Where(t => mediatorHandler.IsAssignableFrom(t) && t != mediatorHandler))
                    {
                        if (!services.Any(s => s.ServiceType == interfaceType && s.ImplementationType == type))
                        {
                            services.AddSingleton(interfaceType, type);
                            if (interfaceType.IsGenericType)
                            {
                                descriptors.Add(new CommandHandlerDescriptor(interfaceType, interfaceType.GenericTypeArguments));
                            }
                        }
                    }
                }
            }

            services.TryAddTransient<IMediator, Mediator>();

            return new MediatorBuilder(services, descriptors);
        }

        /// <summary>
        /// add <seealso cref="IBuilder"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddBuilder(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any()) assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            services.TryAddSingleton<IDirector, Director>();

            var builderType = typeof(IBuilder);


            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes()
                                             .Where(t => t.IsClass &&
                                                    !t.IsAbstract &&
                                                    !t.IsInterface &&
                                                    builderType.IsAssignableFrom(t)))
                {

                    foreach (var interfaceType in type.GetInterfaces()
                                                      .Where(t => builderType.IsAssignableFrom(t) && t != builderType))
                    {
                        if (!services.Any(s => s.ServiceType == interfaceType && s.ImplementationType == type))
                        {
                            services.AddSingleton(interfaceType, type);
                        }
                    }
                }
            }

            return services;
        }
        /// <summary>
        /// add <seealso cref="IResponsibilityProvider{TResponsibilityContext}"/>
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddResponsibilityProvider<TContext>(this IServiceCollection services) where TContext : ResponsibilityContext
        {
            services.TryAddSingleton<IResponsibilityProvider<TContext>, ResponsibilityProvider<TContext>>();
            return services;
        }

        /// <summary>
        /// add <seealso cref="IResponsibility{TContext}"/>
        /// </summary>
        /// <typeparam name="TResponsibility"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddResponsibility<TContext,TResponsibility>(this IServiceCollection services)
            where TResponsibility : class, IResponsibility<TContext>
            where TContext : ResponsibilityContext
        {
            services.AddSingleton<IResponsibility<TContext>, TResponsibility>()
                    .AddResponsibilityProvider<TContext>();
            return services;
        }

        /// <summary>
        /// add <seealso cref="IResponsibility{TContext}"/>
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TResponsibility"></typeparam>
        /// <param name="services"></param>
        /// <param name="responsibility"></param>
        /// <returns></returns>
        public static IServiceCollection AddResponsibility<TContext, TResponsibility>(this IServiceCollection services,TResponsibility responsibility)
            where TResponsibility : class, IResponsibility<TContext>
            where TContext : ResponsibilityContext
        {
            services.AddSingleton<IResponsibility<TContext>>(responsibility)
                    .AddResponsibilityProvider<TContext>();
            return services;
        }
    }
}
