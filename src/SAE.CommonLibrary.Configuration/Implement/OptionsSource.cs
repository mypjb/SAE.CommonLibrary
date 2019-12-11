using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public class OptionsSource : IOptionsSource
    {
        private ConcurrentDictionary<string, OptionsContext> _store;
        private readonly IOptionsProvider _optionsProvider;

        public OptionsSource(IOptionsProvider optionsProvider)
        {
            this._store = new ConcurrentDictionary<string, OptionsContext>();
            this._optionsProvider = optionsProvider;
        }

        public async Task<TOptions> GetAsync<TOptions>(string name) where TOptions : class, new()
        {
            var context = this._store.GetOrAdd(name, key =>
             {
                 var context = new OptionsContext(key, typeof(TOptions));
                 return context;
             });

            if (!context.Complete)
            {
                await this._optionsProvider.HandleAsync(context);
            }

            if (!context.Complete)
            {
                context.Options = new TOptions();
            }

            return (TOptions)context.Options;
        }

        public async Task SaveAsync<TOptions>(string name, TOptions options) where TOptions : class, new()
        {
            var context = this._store.GetOrAdd(name, key =>
            {
                var context = new OptionsContext(key, typeof(TOptions));
                return context;
            });

            await context.Provider.SaveAsync(context.Name, options);
        }
    }
}
