using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// logging extension
    /// </summary>
    public static class SAELoggingDependencyInjectionExtension
    {
        /// <summary>
        /// add <paramref name="loggingFactory"/> imp
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogger<TLoggingFactory>(this IServiceCollection serviceCollection) where TLoggingFactory : class, ILoggingFactory
        {
            if (serviceCollection.IsAddLoggerFactory())
            {
                serviceCollection.AddSingleton<ILoggingFactory, TLoggingFactory>();
            }
            else
            {
                serviceCollection.TryAddSingleton<ILoggingFactory, TLoggingFactory>();
            }

            serviceCollection.TryAddSingleton(typeof(ILogging<>), typeof(Logging<>));

            return serviceCollection;
        }

        /// <summary>
        /// add empty logger
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultLogger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogger<EmptyLoggingFactory>();
            return serviceCollection;
        }
        /// <summary>
        /// <see cref="EmptyLoggingFactory"/> is register?
        /// </summary>
        /// <param name="services"></param>
        public static bool IsAddLoggerFactory(this IServiceCollection services)
        {
            return !services.Any(s => s.ServiceType == typeof(ILoggingFactory) &&
                                 s.ImplementationType != typeof(EmptyLoggingFactory));
        }
    }

}
