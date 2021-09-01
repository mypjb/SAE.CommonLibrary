using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using SAE.CommonLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MediatorBehaviorDependencyInjectionExtension
    {
        public static IMediatorBehaviorBuilder AddMediatorBehavior(this IServiceCollection services)
        {
            return new MediatorBehaviorBuilder(services);
        }

        /// <summary>
        /// <para>Add full caching</para>
        /// <para>* If you don't know what you're doing, don't register this method</para>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching(this IMediatorBehaviorBuilder builder)
        {
            var serviceType = typeof(IPipelineBehavior<,>);
            var implementationType = typeof(CachingPipelineBehavior<,>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// Add single caching
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(CachingPipelineBehavior<TCommand, TResponse>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// Add delete caching behavior
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddDeleteCaching<TCommand>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand>);
            var implementationType = typeof(DeleteCachingPipelineBehavior<TCommand>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// Add single caching
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddDeleteCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder)
             where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(DeleteCachingPipelineBehavior<TCommand, TResponse>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// Add update caching behavior
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddUpdateCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(UpdateCachingPipelineBehavior<TCommand, TResponse>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }

        /// <summary>
        /// <para>Add update caching behavior</para>
        /// <para>* If you don't know what you're doing, don't register this method</para>
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddUpdateCaching(this IMediatorBehaviorBuilder builder)
        {
            var serviceType = typeof(IPipelineBehavior<,>);
            var implementationType = typeof(UpdateCachingPipelineBehavior<,>);
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }

        /// <summary>
        ///  Add mediator pipeline behavior
        /// </summary>
        /// <typeparam name="TPipelineBehavior"></typeparam>
        /// <typeparam name="TImplementationPipelineBehavior"></typeparam>
        /// <param name="builder"></param>
        /// <param name="pipelineBehaviorType"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddPipelineBehavior<TPipelineBehavior, TImplementationPipelineBehavior>(this IMediatorBehaviorBuilder builder)
            where TPipelineBehavior : IPipelineBehavior
            where TImplementationPipelineBehavior : TPipelineBehavior
        {
            return builder.AddPipelineBehavior(typeof(TPipelineBehavior),typeof(TImplementationPipelineBehavior));
        }

        public static IMediatorBehaviorBuilder AddPipelineBehavior(this IMediatorBehaviorBuilder builder,
                                                                   Type pipelineBehaviorServiceType, 
                                                                   Type pipelineBehaviorImplementationType)
        {
            if (!builder.Services.IsRegister(pipelineBehaviorServiceType, pipelineBehaviorImplementationType))
            {
                builder.Services.AddSingleton(pipelineBehaviorServiceType, pipelineBehaviorImplementationType);
            }
            return builder.AddDefault();
        }

        /// <summary>
        /// Add default service
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IMediatorBehaviorBuilder AddDefault(this IMediatorBehaviorBuilder builder)
        {
            builder.Services.AddSaeMemoryDistributedCache();
            builder.Services.TryAddSingleton<ICacheIdentityService, DefaultCacheIdentityService>();
            return builder;
        }
    }
}
