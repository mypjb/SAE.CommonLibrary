using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// 根据<paramref name="name"/>获得<typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsSource"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TOptions Get<TOptions>(this IOptionsSource optionsSource, string name) where TOptions : class, new()
        {
            var options = optionsSource.GetAsync<TOptions>(name)
                                       .GetAwaiter()
                                       .GetResult();
            return options;
        }

        /// <summary>
        /// 获得<typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsSource"></param>
        /// <returns></returns>
        public static TOptions Get<TOptions>(this IOptionsSource optionsSource) where TOptions : class, new()
        {
            return optionsSource.Get<TOptions>(typeof(TOptions).Name);
        }
        /// <summary>
        /// 获得<typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsSource"></param>
        /// <returns></returns>
        public static Task<TOptions> GetAsync<TOptions>(this IOptionsSource optionsSource) where TOptions : class, new()
        {
            return optionsSource.GetAsync<TOptions>(typeof(TOptions).Name);
        }
    }
}
