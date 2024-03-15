using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// autofac 注入配置
    /// </summary>
    public static class DIDependencyInjectionExtension
    {
        /// <summary>
        /// 注入autofac依赖容器
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <param name="delegate">容器委托</param>
        /// <returns><see cref="IServiceProvider"/></returns>
        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, Action<ContainerBuilder> @delegate = null)
        {
            var builder = new ContainerBuilder();
            @delegate?.Invoke(builder);
            return serviceDescriptors.BuildAutofacProvider(builder);
        }
        /// <summary>
        /// 注入autofac依赖容器
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <param name="builder">容器构建对象</param>
        /// <returns><see cref="IServiceProvider"/></returns>
        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, ContainerBuilder builder)
        {
            builder.Populate(serviceDescriptors);
            var container = builder.Build();
            return serviceDescriptors.BuildAutofacProvider(container);
        }
        /// <summary>
        /// 注入autofac依赖容器
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <param name="container">容器对象</param>
        /// <returns><see cref="IServiceProvider"/></returns>
        public static IServiceProvider BuildAutofacProvider(this IServiceCollection serviceDescriptors, IContainer container)
        {
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }

        /// <summary>
        /// 注册服务门面<seealso cref="ServiceFacade"/>
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns><paramref name="serviceDescriptors"/></returns>
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
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns><paramref name="serviceProvider"/></returns>
        public static IServiceProvider UseServiceFacade(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<ServiceFacade>();
            return serviceProvider;
        }

        /// <summary>
        /// 是否注册
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <param name="ServiceType">服务类型</param>
        /// <returns>true:已注册</returns>
        public static bool IsRegister(this IServiceCollection serviceDescriptors, Type ServiceType)
        {
            return serviceDescriptors.Any(s => s.ServiceType == ServiceType);
        }

        /// <summary>
        /// 判断<paramref name="ServiceType"/>是否有<paramref name="implementationType"/>实现
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <returns>true:已注册</returns>
        public static bool IsRegister(this IServiceCollection serviceDescriptors, Type ServiceType, Type implementationType)
        {
            return serviceDescriptors.Any(s => s.ServiceType == ServiceType &&
                                          s.ImplementationType == implementationType);
        }


        /// <summary>
        /// 是否注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns>true:已注册</returns>
        public static bool IsRegister<TService>(this IServiceCollection serviceDescriptors) where TService : class
        {
            return serviceDescriptors.IsRegister(typeof(TService));
        }
        /// <summary>
        /// <typeparamref name="TService"/> is register<typeparamref name="TImplementation"/> imp
        /// </summary>
        /// <typeparam name="TService">接口</typeparam>
        /// <typeparam name="TImplementation">服务实现</typeparam>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns>true:已注册</returns>
        public static bool IsRegister<TService, TImplementation>(this IServiceCollection serviceDescriptors)
            where TService : class 
            where TImplementation : TService
        {
            return serviceDescriptors.IsRegister(typeof(TService), typeof(TImplementation));
        }
    }
}
