using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
namespace SAE.CommonLibrary.DependencyInjection
{
    /// <summary>
    /// 服务提供者扩展
    /// </summary>
    public static class ServiceProviderExtension
    {
        /// <summary>
        /// <typeparamref name="TService"/>是否注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>true:已注册</returns>
        public static bool IsRegistered<TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetService(typeof(TService)) != null;
        }
        /// <summary>
        /// <typeparamref name="TService"/>是否注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="service">服务对象</param>
        /// <returns>true:已注册</returns>
        public static bool TryGetService<TService>(this IServiceProvider serviceProvider, out TService service)
        {
            service = serviceProvider.GetService<TService>();
            return service != null;
        }
        /// <summary>
        /// <typeparamref name="TService"/>是否注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="services">服务对象集合</param>
        /// <returns>true:已注册</returns>
        public static bool TryGetServices<TService>(this IServiceProvider serviceProvider, out IEnumerable<TService> services)
        {
            services = serviceProvider.GetServices<TService>();
            return services != null && services.Any();
        }
    }
}
