using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    public interface IOptionsConfigure<TOptions> where TOptions : class
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="options"></param>
        void Configure(TOptions options);
    }

    internal class OptionsConfigure<TOptions> : IOptionsConfigure<TOptions> where TOptions : class
    {
        private readonly Action<TOptions> _configure;

        public OptionsConfigure(Action<TOptions> configure)
        {
            this._configure = configure;
        }
        public void Configure(TOptions options)
        {
            this._configure?.Invoke(options);
        }
    }
}
