using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIDependencyInjectionExtension
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
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }

        /// <summary>
        /// 注册服务门面<seealso cref="ServiceFacade"/>
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceFacade(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton(provider =>
            {
                return new ServiceFacade(provider);
            });
            return serviceDescriptors;
        }

        /// <summary>
        /// 初始化<seealso cref="ServiceFacade"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseServiceFacade(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<ServiceFacade>();
            return serviceProvider;
        }

        /// <summary>
        /// 注册服务提供程序<seealso cref="ServiceFacade"/>
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceFacade(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.ApplicationServices.GetService<ServiceFacade>();
            return applicationBuilder;
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
        /// 判断<paramref name="ServiceType"/>是否有<paramref name="implementationType"/>实现
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <returns></returns>
        public static bool IsRegister(this IServiceCollection serviceDescriptors, Type ServiceType, Type implementationType)
        {
            return serviceDescriptors.Any(s => s.ServiceType == ServiceType &&
                                          s.ImplementationType == implementationType);
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
        /// <summary>
        /// <typeparamref name="TService"/> is register<typeparamref name="TImplementation"/> imp
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static bool IsRegister<TService, TImplementation>(this IServiceCollection serviceDescriptors)
            where TService : class 
            where TImplementation : TService
        {
            return serviceDescriptors.IsRegister(typeof(TService), typeof(TImplementation));
        }
    }
}
