using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace SAE.CommonLibrary
{
    public class ServiceFacade
    {

        public ServiceFacade(IServiceProvider serviceProvider)
        {
            asyncLocal.Value = serviceProvider;
            defaultProvider = serviceProvider;
        }
        //public static ConcurrentDictionary<string, object> _storge = new ConcurrentDictionary<string, object>();

        private static AsyncLocal<IServiceProvider> asyncLocal = new AsyncLocal<IServiceProvider>();
        private static IServiceProvider defaultProvider;
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
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public static TService GetService<TService>() where TService : class
        {
            //var key = typeof(TService).GUID.ToString();
            //return (TService)_storge.GetOrAdd(key, s => ServiceProvider.GetService<TService>());
            return ServiceProvider == null ? null : ServiceProvider.GetService<TService>();
        }
    }
}
