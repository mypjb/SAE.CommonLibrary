using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// 选项监听
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    public interface IOptionsMonitor<TOption> where TOption : class
    {
        TOption Option { get; }
        void OnChange(Action<TOption> option);
    }
}
