using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Abstract.Mediator
{
    /// <summary>
    /// 中介者
    /// </summary>
    public class DefaultMediator : IMediator
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _cache;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DefaultMediator(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._cache = new ConcurrentDictionary<string, object>();
        }

        /// <inheritdoc/>
        public async Task<object> SendAsync(object command, Type commandType, Type responseType)
        {
            var key = $"{commandType}_{responseType}";

            var wrapper = this._cache.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(DefaultRequestHandlerWrapper<,>)
                                .MakeGenericType(commandType, responseType), this._serviceProvider);
            });

            var response = await((RequestHandlerWrapper)wrapper).InvokeAsync(command);

            return response;
        }
        /// <inheritdoc/>
        public async Task SendAsync(object command, Type commandType)
        {
            var key = commandType.ToString();

            var wrapper = this._cache.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(DefaultCommandHandlerWrapper<>)
                                .MakeGenericType(commandType), this._serviceProvider);
            });

            await((CommandHandlerWrapper)wrapper).InvokeAsync(command);
        }
    }
}
