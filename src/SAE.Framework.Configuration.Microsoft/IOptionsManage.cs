using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.Framework.Logging;

namespace SAE.Framework.Configuration.Microsoft
{
    /* 为避免后期维护问题，取消该实现，改为将对应服务附加到options，以应对多租户配置。
    以下有两种方式作为替代
    配置附加到options上
    public class Options
    {
        public string Connection{get;set;}
        public IDatabase GetDatabase()
        {
            return new Database(this.Connection);
        }
    }
    使用PostConfigure((a,b)=>
    {
        //配置
    }
    )
    */
    // /// <summary>
    // /// 配置管理
    // /// </summary>
    // /// <typeparam name="TOptions"></typeparam>
    // /// <typeparam name="TService"></typeparam>
    // public interface IOptionsManage<TOptions, TService> where TOptions : class where TService : class
    // {
    //     /// <summary>
    //     /// 获得当前服务
    //     /// </summary>
    //     TService Get();
    //     /// <summary>
    //     /// 配置更改
    //     /// </summary>
    //     event Action<TOptions, TService> OnChange;
    //     /// <summary>
    //     /// 配置服务
    //     /// </summary>
    //     event Func<TOptions, TService> OnConfigure;
    // }

    // /// <summary>
    // /// <see cref="IOptionsManage{TOptions, TService}"/>默认实现
    // /// </summary>
    // /// <typeparam name="TOptions"></typeparam>
    // /// <typeparam name="TService"></typeparam>
    // /// <inheritdoc/>
    // public class OptionsManager<TOptions, TService> : IOptionsManage<TOptions, TService> where TOptions : class where TService : class
    // {
    //     private readonly IOptionsMonitor<TOptions> _monitor;
    //     private readonly ILogging<OptionsManager<TOptions, TService>> _logging;
    //     private readonly ConcurrentDictionary<TOptions, TService> _cache;
    //     public OptionsManager(IOptionsMonitor<TOptions> monitor,
    //                           ILogging<OptionsManager<TOptions, TService>> logging)
    //     {
    //         this._cache = new ConcurrentDictionary<TOptions, TService>();
    //         this._monitor = monitor;
    //         this._logging = logging;
    //         this._monitor.OnChange(this.Change);
    //     }

    //     /// <summary>
    //     /// 配置初始化时触发的函数
    //     /// </summary>
    //     /// <param name="options"></param>
    //     private TService Configure(TOptions options)
    //     {
    //         this._logging.Info($"配置变更");
    //         return this.OnConfigure.Invoke(options);
    //     }
    //     /// <summary>
    //     /// 配置更改时触发的私有函数
    //     /// </summary>
    //     /// <param name="options"></param>
    //     private void Change(TOptions options)
    //     {
    //         var dict = this._cache.ToArray();
    //         this._logging.Info($"清理配置数量({dict.Length})");
    //         this._cache.Clear();
    //         this._logging.Info("配置清理完成");
    //         if (this.OnChange == null)
    //             return;

    //         this._logging.Info("开始执行配置变更事件");
    //         foreach (var kv in dict)
    //         {
    //             this._logging.Debug($"开始执行'{kv.Key}'的变更事件");
    //             this.OnChange.Invoke(kv.Key, kv.Value);
    //         }
    //         this._logging.Info("配置变更事件执行完毕");
    //     }
    //     /// <summary>
    //     /// 配置变更事件
    //     /// </summary>
    //     public event Action<TOptions, TService> OnChange;
    //     /// <summary>
    //     /// 配置初始化事件
    //     /// </summary>
    //     public event Func<TOptions, TService> OnConfigure;


    //     /// <summary>
    //     /// 获得服务
    //     /// </summary>
    //     public TService Get()
    //     {
    //         var options = this._monitor.CurrentValue;

    //         if (options == null)
    //         {
    //             this._logging.Warn("当前配置项不存在");
    //             return null;
    //         }

    //         this._logging.Debug("从缓存中查找配置项");

    //         return this._cache.GetOrAdd(options, this.Configure);
    //     }
    // }
}