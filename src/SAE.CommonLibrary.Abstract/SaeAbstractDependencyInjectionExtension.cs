using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SaeAbstractDependencyInjectionExtension
    {

        /// <summary>
        /// 添加中介者
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
                foreach (var type in assembly.GetTypes()
                                             .Where(t => t.IsClass &&
                                                    !t.IsAbstract &&
                                                    !t.IsInterface &&
                                                    mediatorHandler.IsAssignableFrom(t)))
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

            return new MediatorBuilder(services,descriptors);
        }
    }
}
