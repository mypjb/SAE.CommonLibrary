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
        public TService GetService()
        {
            var scope = this._scopeFactory.Get();
            return this._cache.GetOrAdd(scope.Name, this.GetServiceCore);
        }
        /// <summary>
        /// get service core
        /// </summary>
        /// <param name="name"></param>
        private TService GetServiceCore(string name)
        {
            using (this._scopeFactory.GetAsync(name))
            {
                return this._serviceProvider.GetService<TService>();
            }
        }
    }
}