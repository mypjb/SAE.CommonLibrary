using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    public interface IOptionsSource
    {
        /// <summary>
        /// 获得<typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TOptions> GetAsync<TOptions>(string name) where TOptions : class, new();
        /// <summary>
        /// 设置<typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="name"></param>
        Task SaveAsync<TOptions>(string name,TOptions options) where TOptions : class, new();
    }
}
