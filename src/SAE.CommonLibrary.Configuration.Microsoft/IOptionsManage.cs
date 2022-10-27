using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Configuration.Microsoft
{
    /// <summary>
    /// options manage
    /// </summary>
    /// <typeparam name="TOptions">options class</typeparam>
    /// <typeparam name="TService">options setting service class</typeparam>
    public interface IOptionsManage<TOptions, TService> where TOptions : class where TService : class
    {
        /// <summary>
        /// get current service
        /// </summary>
        TService Get();
        /// <summary>
        /// options change event
        /// </summary>
        event Action<TOptions, TService> OnChange;
        /// <summary>
        /// confiure service
        /// </summary>
        event Func<TOptions, TService> OnConfigure;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TService"></typeparam>
    public class OptionsManager<TOptions, TService> : IOptionsManage<TOptions, TService> where TOptions : class where TService : class
    {
        private readonly IOptionsMonitor<TOptions> _monitor;
        private readonly ILogging<OptionsManager<TOptions, TService>> _logging;
        private readonly ConcurrentDictionary<TOptions, TService> _cache;
        public OptionsManager(IOptionsMonitor<TOptions> monitor,
                              ILogging<OptionsManager<TOptions, TService>> logging)
        {
            this._cache = new ConcurrentDictionary<TOptions, TService>();
            this._monitor = monitor;
            this._logging = logging;
            this._monitor.OnChange(this.Change);
        }

        /// <summary>
        /// configure <seealso cref="TService"/>
        /// </summary>
        /// <param name="options"></param>
        private TService Configure(TOptions options)
        {
            this._logging.Info($"config change");
            return this.OnConfigure.Invoke(options);
        }
        /// <summary>
        /// options change function
        /// </summary>
        /// <param name="options"></param>
        private void Change(TOptions options)
        {
            var dict = this._cache.ToArray();
            this._logging.Info($"clear config count({dict.Length})");
            this._cache.Clear();
            this._logging.Info("clear config ok");
            if (this.OnChange == null)
                return;

            this._logging.Info("start exec onChange event");
            foreach (var kv in dict)
            {
                this.OnChange.Invoke(kv.Key, kv.Value);
            }
            this._logging.Info("onChange exec end");
        }

        public event Action<TOptions, TService> OnChange;
        public event Func<TOptions, TService> OnConfigure;


        /// <summary>
        /// get <typeparamref name="TService"/>
        /// </summary>
        public TService Get()
        {
            var options = this._monitor.CurrentValue;

            if (options == null)
            {
                this._logging.Warn("The current options does not exist");
                return null;
            }

            this._logging.Debug("Look up the options from the cache");

            return this._cache.GetOrAdd(options, this.Configure);
        }
    }
}