using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// 选项监听
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptionsMonitor<TOptions> where TOptions : class
    {
        TOptions Options { get; }
        void OnChange(Func<TOptions,Task> options);
        void TriggerChange(TOptions options);
    }
}
