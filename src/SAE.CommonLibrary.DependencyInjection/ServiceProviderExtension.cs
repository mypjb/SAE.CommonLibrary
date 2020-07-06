using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool TryGetService<TService>(this IServiceProvider serviceProvider, out TService service)
        {
            service = serviceProvider.GetService<TService>();
            return service != null;
        }

        public static bool TryGetServices<TService>(this IServiceProvider serviceProvider, out IEnumerable<TService> service)
        {
            service = serviceProvider.GetServices<TService>();
            return service != null && service.Any();
        }
    }
}
