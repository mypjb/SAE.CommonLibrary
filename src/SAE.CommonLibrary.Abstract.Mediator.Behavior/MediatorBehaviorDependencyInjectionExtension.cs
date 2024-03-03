using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using SAE.CommonLibrary.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MediatorBehaviorDependencyInjectionExtension
    {
        /// <summary>
        /// 添加管道配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddMediatorBehavior(this IServiceCollection services)
        {
            return new MediatorBehaviorBuilder(services);
        }

        /// <summary>
        /// <para>为所有命令请求添加缓存</para>
        /// <para>* 如果你不知道这个命令的含义，请勿使用 *</para>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching(this IMediatorBehaviorBuilder builder)
        {
            var serviceType = typeof(IPipelineBehavior<,>);
            var implementationType = typeof(CachingPipelineBehavior<,>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// 为单个命令添加缓存
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(CachingPipelineBehavior<TCommand, TResponse>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// 为单个命令添加缓存移除操作
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddDeleteCaching<TCommand>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand>);
            var implementationType = typeof(DeleteCachingPipelineBehavior<TCommand>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// 为单个命令添加缓存移除操作
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddDeleteCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder)
             where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(DeleteCachingPipelineBehavior<TCommand, TResponse>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }
        /// <summary>
        /// 为单个命令添加缓存更新操作
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddUpdateCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            var serviceType = typeof(IPipelineBehavior<TCommand, TResponse>);
            var implementationType = typeof(UpdateCachingPipelineBehavior<TCommand, TResponse>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }

        /// <summary>
        /// <para>为所有命令请求添加更新操作</para>
        /// <para>* 如果你不知道这个命令的含义，请勿使用 *</para>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddUpdateCaching(this IMediatorBehaviorBuilder builder)
        {
            var serviceType = typeof(IPipelineBehavior<,>);
            var implementationType = typeof(UpdateCachingPipelineBehavior<,>);
            builder.AddDefaultCache();
            return builder.AddPipelineBehavior(serviceType, implementationType);
        }

        /// <summary>
        ///  添加自定义管理行为
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddPipelineBehavior<TPipelineBehavior, TImplementationPipelineBehavior>(this IMediatorBehaviorBuilder builder)
            where TPipelineBehavior : IPipelineBehavior
            where TImplementationPipelineBehavior : TPipelineBehavior
        {
            return builder.AddPipelineBehavior(typeof(TPipelineBehavior), typeof(TImplementationPipelineBehavior));
        }
        /// <summary>
        /// 添加自定义管理行为
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pipelineBehaviorServiceType"></param>
        /// <param name="pipelineBehaviorImplementationType"></param>
        public static IMediatorBehaviorBuilder AddPipelineBehavior(this IMediatorBehaviorBuilder builder,
                                                                   Type pipelineBehaviorServiceType,
                                                                   Type pipelineBehaviorImplementationType)
        {
            if (!builder.Services.IsRegister(pipelineBehaviorServiceType, pipelineBehaviorImplementationType))
            {
                builder.Services.AddSingleton(pipelineBehaviorServiceType, pipelineBehaviorImplementationType);
            }
            return builder;
        }
        /// <summary>
        /// 为所有命令添加重试策略
        /// </summary>
        /// <param name="builder"></param>
        public static IMediatorBehaviorBuilder AddRetry(this IMediatorBehaviorBuilder builder)
        {
            return builder.AddDefaultRetry()
                          .AddPipelineBehavior(typeof(IPipelineBehavior<>), typeof(RetryPipelineBehavior<>))
                          .AddPipelineBehavior(typeof(IPipelineBehavior<,>), typeof(RetryPipelineBehavior<,>));
        }
        public static IMediatorBehaviorBuilder AddRetry<TCommand>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            return builder.AddDefaultRetry()
                          .AddPipelineBehavior(typeof(IPipelineBehavior<TCommand>), typeof(RetryPipelineBehavior<TCommand>));
        }

        /// <summary>
        /// 为单个命令添加重试策略
        /// </summary>
        /// <param name="builder"></param>
        public static IMediatorBehaviorBuilder AddRetry<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            return builder.AddDefaultRetry()
                          .AddPipelineBehavior(typeof(IPipelineBehavior<TCommand, TResponse>), typeof(RetryPipelineBehavior<TCommand, TResponse>));
        }
        /// <summary>
        /// 添加重试默认依赖
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IMediatorBehaviorBuilder AddDefaultRetry(this IMediatorBehaviorBuilder builder)
        {
            builder.Services.AddOptions<RetryPipelineBehaviorOptions>()
                            .Bind(RetryPipelineBehaviorOptions.Option);
            return builder;
        }

        /// <summary>
        /// 添加缓存默认依赖
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IMediatorBehaviorBuilder AddDefaultCache(this IMediatorBehaviorBuilder builder)
        {
            builder.Services.AddSAEMemoryDistributedCache();
            builder.Services.TryAddSingleton<ICacheIdentityService, DefaultCacheIdentityService>();
            return builder;
        }
    }
}
