using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Scope
{
    /// <summary>
    /// <see cref="IScopeWrapper{TService}"/>默认实现
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    public class DefaultScopeWrapper<TService> : IScopeWrapper<TService> where TService : class
    {
        private readonly ConcurrentDictionary<string, TService> _cache;
        private readonly IScopeFactory _scopeFactory;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 默认区域包装器
        /// </summary>
        /// <param name="scopeFactory">区域工厂</param>
        /// <param name="serviceProvider">服务提供者</param>
        public DefaultScopeWrapper(IScopeFactory scopeFactory, IServiceProvider serviceProvider)
        {
            this._cache = new ConcurrentDictionary<string, TService>();
            this._scopeFactory = scopeFactory;
            this._serviceProvider = serviceProvider;
        }
        /// <inheritdoc/>
        public void Clear()
        {
            this._cache.Clear();
        }
        /// <inheritdoc/>
        public TService GetService(string key, Func<TService> constructor)
        {
#warning 在类似与工厂方法下，该函数会由于缓存的原因导致无法切换上下文。
            var scope = this._scopeFactory.Get();
            key = $"{scope.Name}_{key}";
            return this._cache.GetOrAdd(key, scopeName =>
            {
                using (this._scopeFactory.Get(scopeName))
                {
                    return constructor.Invoke();
                }
            });
        }

    }
}