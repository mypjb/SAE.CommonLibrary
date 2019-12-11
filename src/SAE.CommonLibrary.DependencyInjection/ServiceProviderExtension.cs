using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
namespace SAE.CommonLibrary.DependencyInjection
{
    public static class ServiceProviderExtension
    {
        public static bool IsRegistered<TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetService(typeof(TService)) != null;
        }

        public static bool TryGetService<TService>(this IServiceProvider serviceProvider,out TService service)
        {
            service = serviceProvider.GetService<TService>();
            return service != null;
        }
    }
}
