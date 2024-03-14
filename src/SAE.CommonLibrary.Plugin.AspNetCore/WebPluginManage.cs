using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SAE.CommonLibrary.Plugin.AspNetCore
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class WebPluginManage : PluginManage<WebPlugin>
    {
        private const string SearchAssemblyFile = "*.dll";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pluginOptions">插件配置</param>
        public WebPluginManage(PluginOptions pluginOptions) : base(pluginOptions)
        {
        }

        ///<inheritdoc/>
        protected override Dictionary<string, IPlugin> LoadPlugin()
        {
            var storage = new Dictionary<string, IPlugin>();
            if (AppContext.BaseDirectory.Equals(this._pluginDir))
            {

                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, SearchAssemblyFile);

                foreach (var assemblyFile in files)
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (this._pluginType.IsAssignableFrom(type))
                        {
                            if (type.IsAbstract || type.IsInterface)
                            {
                                continue;
                            }
                            var plugin = (IPlugin)Activator.CreateInstance(type);
                            var assemblyName = assembly.GetName();
                            plugin.Status = true;
                            plugin.Version = "v1";
                            plugin.Name = assemblyName.Name;
                            plugin.Path = assembly.Location;
                            plugin.Description = assemblyName.Name;

                            storage[assemblyName.Name] = plugin;
                            break;
                        }
                    }
                }
            }
            else
            {
                storage = base.LoadPlugin();
            }

            return storage;
        }
        ///<inheritdoc/>
        public override Assembly Load(IPlugin plugin)
        {
            if (AppContext.BaseDirectory.Equals(this._pluginDir))
            {
                return null;
            }
            else
            {
                return base.Load(plugin);
            }

        }
        ///<inheritdoc/>
        public override void UnLoad(string name)
        {
            if (!AppContext.BaseDirectory.Equals(this._pluginDir))
            {
                base.UnLoad(name);
            }
        }
        ///<inheritdoc/>
        public override IEnumerable<IPlugin> Plugins
        {
            get
            {
                if (AppContext.BaseDirectory.Equals(this._pluginDir))
                {
                    return this._store.Values;
                }
                return base.Plugins;
            }
        }
    }
}
