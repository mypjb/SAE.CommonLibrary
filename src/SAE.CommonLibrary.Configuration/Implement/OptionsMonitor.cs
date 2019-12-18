using SAE.CommonLibrary.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public sealed class OptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class
    {
        private readonly IServiceProvider _serviceProvider;

        public OptionsMonitor(TOptions options,IServiceProvider serviceProvider)
        {
            this.Options = options;
            this._serviceProvider = serviceProvider;
        }
        public TOptions Options
        {
            get;
        }

        public event Func<TOptions, Task> ChangeEvent;

        public void OnChange(Func<TOptions, Task> options)
        {
            if (options != null)
                this.ChangeEvent += options;
        }

        public void TriggerChange(TOptions options)
        {
            var logging = this._serviceProvider.GetService<ILogging<OptionsMonitor<TOptions>>>();
            logging.Info($"触发更改事件");
            this.ChangeEvent?.Invoke(options);
        }
    }
}
