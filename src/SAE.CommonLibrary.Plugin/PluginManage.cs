using System.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Plugin;

namespace SAE.CommonLibrary.AspNetCore.Plugin
{
    /// <summary>
    /// <seealso cref="IPluginManage"/>实现
    /// </summary>
    /// <typeparam name="TPlugin">插件类型</typeparam>
    public class PluginManage<TPlugin> : IPluginManage where TPlugin : IPlugin
    {
        /// <summary>
        /// 插件目录
        /// </summary>
        protected readonly string _pluginDir;
        /// <summary>
        /// 插件类型
        /// </summary>
        protected readonly Type _pluginType;
        /// <summary>
        /// 插件存储
        /// </summary>
        protected readonly ConcurrentDictionary<string, IPlugin> _store;
        /// <summary>
        /// 插件描述文件
        /// </summary>
        public const string Package = "package.json";
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pluginOptions">插件配置</param>
        public PluginManage(PluginOptions pluginOptions)
        {
            this._pluginType = typeof(TPlugin);
            this._store = new ConcurrentDictionary<string, IPlugin>();

            if (string.IsNullOrWhiteSpace(pluginOptions.Path))
            {
                this._pluginDir = AppContext.BaseDirectory;
            }
            else
            {
                this._pluginDir = this.AbsolutePath(pluginOptions.Path) ?
                                       pluginOptions.Path :
                                       Path.Combine(AppContext.BaseDirectory, pluginOptions.Path);
            }


            var pairs = this.LoadPlugin();

            foreach (var kv in pairs.OrderByDescending(kv => kv.Value.Order))
            {
                this._store[kv.Key] = kv.Value;
            }
        }
        ///<inheritdoc/>
        protected virtual Dictionary<string, IPlugin> LoadPlugin()
        {
            Dictionary<string, IPlugin> pairs = new Dictionary<string, IPlugin>();
            if (Directory.Exists(this._pluginDir))
            {
                Directory.GetDirectories(this._pluginDir)
                         .ForEach(dir =>
                         {
                             var plugin = this.Read(dir);
                             if (plugin != null)
                             {
                                 pairs[plugin.Name] = plugin;
                             }
                         });
            }
            return pairs;
        }
        ///<inheritdoc/>
        protected virtual IPlugin Read(string dir)
        {
            IPlugin plugin = null;
            var packageFile = Path.Combine(dir, Package);
            if (!File.Exists(packageFile))
            {
                return plugin;
            }

            var json = File.ReadAllText(packageFile);

            var proxyPlugin = new ProxyPlugin(json.ToObject<SAE.CommonLibrary.Plugin.Plugin>());

            if (proxyPlugin?.Check() ?? false)
            {
                if (proxyPlugin.Status)
                {
                    var assembly = this.Load(proxyPlugin);

                    if (assembly == null)
                    {
                        return plugin;
                    }

                    var pluginType = assembly.GetTypes()
                                             .FirstOrDefault(
                                                s => this._pluginType.IsAssignableFrom(s));
                    if (pluginType != null)
                    {
                        var tPlugin = this.CreateInstance(pluginType);
                        proxyPlugin.Extension(tPlugin);
                    }
                }
                plugin = proxyPlugin;
            }

            return plugin;
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type">插件类型</param>
        /// <returns>插件接口</returns>
        protected virtual IPlugin CreateInstance(Type type)
        {
            return (IPlugin)Activator.CreateInstance(type);
        }
        /// <summary>
        /// 是否为绝对路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true:绝对路径</returns>
        private bool AbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }
        ///<inheritdoc/>
        public virtual IEnumerable<IPlugin> Plugins =>
               this._store.Values.OfType<ProxyPlugin>().Select(s => s.plugin).ToArray();
        ///<inheritdoc/>
        public virtual Assembly Load(IPlugin plugin)
        {
            if (!Path.IsPathRooted(plugin.Path))
            {
                plugin.Path = Path.Combine(this._pluginDir, plugin.Name, plugin.Path);
            }

            if (!(plugin is ProxyPlugin))
            {
                plugin = new ProxyPlugin(plugin);
            }

            var proxy = (ProxyPlugin)this._store.AddOrUpdate(plugin.Name, plugin, (k, p) =>
                 {
                     if (p.Version != plugin.Version)
                     {
                         return plugin;
                     }
                     else
                     {
                         return p;
                     }
                 });
            return proxy.Register();
        }
        ///<inheritdoc/>
        public virtual void UnLoad(string name)
        {
            IPlugin plugin;
            if (this._store.TryRemove(name, out plugin))
            {
                ((ProxyPlugin)plugin).Delete();
            }
        }
    }
}
