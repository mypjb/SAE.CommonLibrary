using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.MessageQueue;
using SAE.CommonLibrary.MessageQueue.Memory;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageQueueDependencyInjectionExtension
    {
        /// <summary>
        /// 添加MQ内存实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IMessageQueueBuilder AddMemoryMessageQueue(this IServiceCollection services)
        {
            services.TryAddSingleton<IMessageQueue, MemoryMessageQueue>();
            services.AddDefaultLogger();
            return new MessageQueueBuilder(services);
        }
        /// <summary>
        /// 扫描程序集<paramref name="assemblies"/>并注册继承自<see cref="IHandler{TMessage}"/>的接口实现
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies">实现了<see cref="IHandler{TMessage}"/>接口的程序集</param>
        public static IMessageQueueBuilder AddHandler(this IMessageQueueBuilder builder, params Assembly[] assemblies)
        {
            var services = builder.Services;

            if (assemblies == null || !assemblies.Any())
            {
                assemblies = new Assembly[1] { Assembly.GetCallingAssembly() };
            }

            var handlerType = typeof(IHandler);

            for (int i = 0; i < assemblies.Length; i++)
            {
                foreach (Type type in from t in assemblies[i].GetAssignableFrom(handlerType)
                                      where t.IsClass && !t.IsAbstract && !t.IsInterface
                                      select t)
                {
                    foreach (Type interfaceType in from t in type.GetInterfaces()
                                                   where handlerType.IsAssignableFrom(t) && t != handlerType
                                                   select t)
                    {
                        if (!services.Any((ServiceDescriptor s) => s.ServiceType == interfaceType && s.ImplementationType == type))
                        {
                            services.AddSingleton(interfaceType, type);
                        }
                    }
                }
            }
            return new MessageQueueBuilder(services);
        }
    }
}
