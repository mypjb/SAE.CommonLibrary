using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        
        /// <summary>
        /// 添加中介者
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediator(this IServiceCollection serviceDescriptors, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any()) assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            var mediatorHandler = typeof(IMediatorHandler);

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
                        serviceDescriptors.AddSingleton(interfaceType, type);
                    }
                }
            }

            serviceDescriptors.TryAddTransient<IMediator, Mediator>();

            return serviceDescriptors;
        }
    }
}
