using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// <inheritdoc/>
    /// <seealso cref="IScopeWrapper{TService}"/> default implement
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class DefaultScopeWrapper<TService> : IScopeWrapper<TService> where TService : class
    {
        private readonly ConcurrentDictionary<string, TService> _cache;
        private readonly IScopeFactory _scopeFactory;
        private readonly IServiceProvider _serviceProvider;

        public DefaultScopeWrapper(IScopeFactory scopeFactory, IServiceProvider serviceProvider)
        {
            this._cache = new ConcurrentDictionary<string, TService>();
            this._scopeFactory = scopeFactory;
            this._serviceProvider = serviceProvider;
        }

        public void Clear()
        {
            this._cache.Clear();
        }

        public TService GetService(Func<TService> constructor)
        {
            #warning 在类似与工厂方法下，该函数会由于缓存的原因导致无法切换上下文。
            var scope = this._scopeFactory.Get();
            return this._cache.GetOrAdd(scope.Name, scopeName =>
            {
                using (this._scopeFactory.Get(scopeName))
                {
                    return constructor.Invoke();
                }
            });
        }

    }
}