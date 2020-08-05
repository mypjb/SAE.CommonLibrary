using System.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Plugin.AspNetCore
{
    public class PluginManage<TPlugin> : IPluginManage where TPlugin : IPlugin
    {
        private readonly string _pluginDir;
        private readonly Type _plutinType;
        private ConcurrentDictionary<string, IPlugin> _store;
        public const string Package = "package.json";
        public PluginManage(PluginOptions pluginOptions)
        {
            this._plutinType = typeof(TPlugin);
            this._store = new ConcurrentDictionary<string, IPlugin>();
            this._pluginDir = this.AbsolutePath(pluginOptions.Path) ?
                                    pluginOptions.Path :
                                    Path.Combine(AppContext.BaseDirectory, pluginOptions.Path);
            this.LoadPlugin();
        }

        protected virtual void LoadPlugin()
        {
            if (Directory.Exists(this._pluginDir))
            {
                Directory.GetDirectories(this._pluginDir)
                         .ForEach(dir =>
                         {
                             var plugin = this.Read(dir);
                             if (plugin != null)
                             {
                                 this._store[plugin.Name] = plugin;
                             }
                         });
            }
        }

        protected virtual IPlugin Read(string dir)
        {
            IPlugin plugin = null;
            var packageFile = Path.Combine(dir, Package);
            if (!File.Exists(packageFile))
            {
                return plugin;
            }

            var json = File.ReadAllText(packageFile);

            var proxyPlugin = new ProxyPlugin(json.ToObject<Plugin>());

            if (proxyPlugin?.Check() ?? false)
            {
                if (proxyPlugin.Status)
                {
                    var assembly = this.Load(proxyPlugin);
                    
                    var pluginType = assembly.GetTypes()
                                             .FirstOrDefault(
                                                s => this._plutinType.IsAssignableFrom(s));
                    if (pluginType != null)
                    {
                        var tPlugin = (IPlugin)Activator.CreateInstance(pluginType);
                        proxyPlugin.Extension(tPlugin);
                    }
                }
                plugin = proxyPlugin;
            }

            return plugin;
        }

        private bool AbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }

        public IEnumerable<IPlugin> Plugins =>
               this._store.Values.OfType<ProxyPlugin>().Select(s => s.plugin).ToArray();

        public Assembly Load(IPlugin plugin)
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
        public void UnLoad(string name)
        {
            IPlugin plugin;
            if (this._store.TryRemove(name, out plugin))
            {
                ((ProxyPlugin)plugin).Delete();
            }
        }
    }
}
