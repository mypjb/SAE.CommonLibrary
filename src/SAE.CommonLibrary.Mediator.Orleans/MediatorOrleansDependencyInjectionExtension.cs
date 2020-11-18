using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Mediator.Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MediatorOrleansDependencyInjectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IMediatorBuilder AddMediatorOrleans(this IMediatorBuilder builder)
        {
            builder.Services.AddNlogLogger()
                            .AddMicrosoftLogging()
                            .AddOptions<OrleansOptions>(OrleansOptions.Option)
                            .Bind()
                            .Services
                            .TryAddSingleton<IGrainCommandHandler, GrainCommandHandler>();
            return builder;
        }
        /// <summary>
        /// 添加对<seealso cref="IMediator"/>Proxy实现
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBuilder AddMediatorOrleansProxy(this IMediatorBuilder builder)
        {
            return builder.AddMediatorOrleansSilo()
                          .AddMediatorOrleansClient();
        }

        /// <summary>
        /// 添加Orleans Silo
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBuilder AddMediatorOrleansSilo(this IMediatorBuilder builder)
        {
            var identitys = new Dictionary<string, Assembly>();

            builder.Descriptors.ForEach(descriptor =>
            {
                var identity = Utility.GetIdentity(descriptor.CommandType);
                if (!identitys.ContainsKey(identity))
                {
                    identitys.Add(identity, descriptor.CommandType.Assembly);
                }
            });

            builder.Services.AddOptions<OrleansOptions>(OrleansOptions.Option)
                            .Bind()
                            .PostConfigure(options =>
                            {
                                foreach (var kv in identitys)
                                    options.GrainNames.TryAdd(kv.Key, kv.Value);
                            });
            builder.AddMediatorOrleans()
                   .Services.TryAddSingleton<ISiloFactory, SiloFactory>();
            return builder;
        }

        /// <summary>
        /// 添加Orleans Client
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMediatorBuilder AddMediatorOrleansClient(this IMediatorBuilder builder)
        {
            builder.AddMediatorOrleans()
                   .Services.TryAddSingleton<IProxyCommandHandlerProvider, ProxyCommandHandlerProvider>();
            return builder;
        }

        /// <summary>
        /// 使用基于Orleans<seealso cref="IMediator"/>代理
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IServiceProvider UseMediatorOrleansSilo(this IServiceProvider provider)
        {
            var factory = provider.GetService<ISiloFactory>();
            return provider;
        }

        /// <summary>
        /// 使用基于Orleans<seealso cref="IMediator"/>代理
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMediatorOrleansSilo(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.UseMediatorOrleansSilo();
            return builder;
        }
    }
}
