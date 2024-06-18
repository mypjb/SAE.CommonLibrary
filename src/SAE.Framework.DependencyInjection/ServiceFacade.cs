using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace SAE.Framework
{
    /// <summary>
    /// 服务门面
    /// </summary>
    public class ServiceFacade
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>

        public ServiceFacade(IServiceProvider serviceProvider)
        {
            asyncLocal.Value = serviceProvider;
            defaultProvider = serviceProvider;
        }
        //public static ConcurrentDictionary<string, object> _storge = new ConcurrentDictionary<string, object>();

        private static AsyncLocal<IServiceProvider> asyncLocal = new AsyncLocal<IServiceProvider>();
        private static IServiceProvider defaultProvider;
        /// <summary>
        /// 返回当前线程的服务提供者
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                return asyncLocal.Value ?? defaultProvider;
            }
        }

        /// <summary>
        /// 获得<typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="TService">服务接口</typeparam>
        /// <returns><typeparamref name="TService"/>对象</returns>
        public static TService GetService<TService>() where TService : class
        {
            //var key = typeof(TService).GUID.ToString();
            //return (TService)_storge.GetOrAdd(key, s => ServiceProvider.GetService<TService>());
            return ServiceProvider == null ? null : ServiceProvider.GetService<TService>();
        }
    }
}
