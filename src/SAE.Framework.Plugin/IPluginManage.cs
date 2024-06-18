using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Plugin
{
    /// <summary>
    /// 插件管理接口
    /// </summary>
    public interface IPluginManage
    {
        /// <summary>
        /// 插件列表
        /// </summary>
        IEnumerable<IPlugin> Plugins { get; }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns>程序集</returns>
        Assembly Load(IPlugin plugin);
        /// <summary>
        /// 卸载程序集
        /// </summary>
        /// <param name="name">插件名称</param>
        void UnLoad(string name);
    }
}
