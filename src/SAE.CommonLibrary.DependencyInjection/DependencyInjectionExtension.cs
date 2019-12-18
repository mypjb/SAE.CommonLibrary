﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using SAE.CommonLibrary;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, Action<ContainerBuilder> @delegate = null)
        {
            var builder = new ContainerBuilder();
            @delegate?.Invoke(builder);
            return serviceDescriptors.BuildAutofacProvider(builder);
        }
        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, ContainerBuilder builder)
        {
            builder.Populate(serviceDescriptors);
            var container = builder.Build();
            return serviceDescriptors.BuildAutofacProvider(container);
        }

        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, IContainer container)
        {
            var serviceProvider= new AutofacServiceProvider(container);
            serviceProvider.SetServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        /// 注册服务提供程序<seealso cref="IServiceProvider"/>
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceProvider(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton(provider =>
            {
                provider.SetServiceProvider();
                return provider;
            });
            return serviceDescriptors;
        }

        /// <summary>
        /// 设置服务提供程序至<seealso cref="ServiceFacade.ServiceProvider"/>属性
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceProvider SetServiceProvider(this IServiceProvider serviceProvider)
        {
            ServiceFacade.ServiceProvider = serviceProvider;
            return serviceProvider;
        }
        /// <summary>
        /// 是否注册
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="ServiceType">服务类型</param>
        /// <returns></returns>
        public static bool IsRegister(this IServiceCollection serviceDescriptors, Type ServiceType)
        {
            return serviceDescriptors.Any(s => s.ServiceType == ServiceType);
        }
        /// <summary>
        /// 是否注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static bool IsRegister<TService>(this IServiceCollection serviceDescriptors) where TService : class
        {
            return serviceDescriptors.IsRegister(typeof(TService));
        }
    }
}
