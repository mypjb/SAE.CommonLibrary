using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public class OptionsSource : IOptionsSource
    {
        private ConcurrentDictionary<string, OptionsContext> _store;
        private readonly IResponsibility<OptionsContext> _responsibility;
        private readonly IServiceProvider _serviceProvider;
        private readonly Lazy<ILogging<OptionsSource>> _logging;

        public OptionsSource(IEnumerable<IOptionsProvider> optionsProviders,
                             IServiceProvider serviceProvider,
                             Lazy<ILogging<OptionsSource>> logging)
        {
            this._store = new ConcurrentDictionary<string, OptionsContext>();
            this._responsibility = new ProxyResponsibility<OptionsContext>(optionsProviders);
            this._serviceProvider = serviceProvider;
            this._logging = logging;
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
                try
                {
                    await this._responsibility.HandleAsync(context);

                }
                catch (Exception ex)
                {
                    _logging.Value.Error(ex, $"在获取'{context.Name}'配置的时候发生异常");
                    throw ex;
                }


                if (context.Complete)
                {
                    context.Provider.OnChange += async () =>
                    {
                        var monitor = this._serviceProvider.GetService<IOptionsMonitor<TOptions>>();
                        try
                        {
                            await context.Provider.HandleAsync(context);
                        }
                        catch (Exception ex)
                        {
                            _logging.Value.Error(ex, $"在'{context.Name}'配置发生变动并重新解析时触发异常");
                            return;
                        }
                        monitor.TriggerChange((TOptions)context.Options);
                    };
                }
                else
                {
                    return new TOptions();
                }
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

            var message = $"使用'{context.Provider.GetType().Name}'保存'{name}'数据";

            this._logging.Value.Info(message);
            try
            {
                await context.Provider.SaveAsync(context.Name, options);
            }catch(Exception ex)
            {
                this._logging.Value.Error(ex, $"{message},时发生异常");
            }
            
        }
    }
}
