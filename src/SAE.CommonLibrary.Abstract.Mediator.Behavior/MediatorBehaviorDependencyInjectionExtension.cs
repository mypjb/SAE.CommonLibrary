using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
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
        /// Add full caching
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching(this IMediatorBehaviorBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CachingPipelineBehavior<,>));
            builder.Services.AddSaeMemoryDistributedCache();
            return builder;
        }
        /// <summary>
        /// Add single caching
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddCaching<TCommand, TResponse>(this IMediatorBehaviorBuilder builder) where TCommand : class
        {
            builder.Services.AddSingleton<IPipelineBehavior<TCommand, TResponse>, CachingPipelineBehavior<TCommand, TResponse>>();
            builder.Services.AddSaeMemoryDistributedCache();
            return builder;
        }
        /// <summary>
        /// Add mediator pipeline behavior
        /// </summary>
        /// <typeparam name="TPipelineBehavior"></typeparam>
        /// <param name="builder"></param>
        /// <param name="pipelineBehaviorType"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddPipelineBehavior<TPipelineBehavior>(this IMediatorBehaviorBuilder builder, Type pipelineBehaviorType) where TPipelineBehavior : IPipelineBehavior
        {
            builder.Services.AddSingleton(typeof(TPipelineBehavior), pipelineBehaviorType);
            return builder;
        }
        /// <summary>
        ///  Add mediator pipeline behavior
        /// </summary>
        /// <typeparam name="TPipelineBehavior"></typeparam>
        /// <typeparam name="ImpPipelineBehavior"></typeparam>
        /// <param name="builder"></param>
        /// <param name="pipelineBehaviorType"></param>
        /// <returns></returns>
        public static IMediatorBehaviorBuilder AddPipelineBehavior<TPipelineBehavior, ImpPipelineBehavior>(this IMediatorBehaviorBuilder builder, Type pipelineBehaviorType)
            where TPipelineBehavior : IPipelineBehavior
            where ImpPipelineBehavior : TPipelineBehavior
        {
            return builder.AddPipelineBehavior<TPipelineBehavior>(typeof(ImpPipelineBehavior));
        }
    }
}
