using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace SAE.Framework.Plugin
{
    /// <summary>
    /// 插件代理实现
    /// </summary>
    internal class ProxyPlugin : IPlugin
    {
        internal IPlugin plugin;
        private readonly Lazy<PluginLoadContext> _loadContext;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="plugin">插件对象</param>
        public ProxyPlugin(IPlugin plugin)
        {
            this.plugin = plugin;
            if (this.plugin is ProxyPlugin)
            {
                _loadContext = (this.plugin as ProxyPlugin)._loadContext;
            }
            else
            {
                _loadContext = new Lazy<PluginLoadContext>(InitContext);
            }

        }
        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <returns></returns>
        private PluginLoadContext InitContext()
        {
            return new PluginLoadContext(this);
        }
        /// <inheritdoc/>
        public string Name { get => plugin.Name; set => plugin.Name = value; }
        /// <inheritdoc/>
        public string Description { get => plugin.Description; set => plugin.Description = value; }
        /// <inheritdoc/>
        public string Version { get => plugin.Version; set => plugin.Version = value; }
        /// <inheritdoc/>
        public string Path { get => plugin.Path; set => plugin.Path = value; }
        /// <inheritdoc/>
        public bool Status { get => plugin.Status; set => plugin.Status = value; }
        /// <inheritdoc/>
        public int Order { get => plugin.Order; set => plugin.Order = value; }
        /// <summary>
        /// 注册插件
        /// </summary>
        /// <returns>插件程序集</returns>
        internal Assembly Register()
        {
            return this._loadContext.Value.Load(this.Path);
        }
        /// <summary>
        /// 删除插件
        /// </summary>
        internal void Delete()
        {
            this._loadContext.Value.Unload();
        }
        /// <summary>
        /// 检查插件是否合规
        /// </summary>
        /// <returns></returns>
        internal bool Check()
        {

            return !(string.IsNullOrWhiteSpace(this.Name) ||
                     string.IsNullOrWhiteSpace(this.Version) ||
                     string.IsNullOrWhiteSpace(this.Path));
        }
        /// <summary>
        /// 合并插件对象
        /// </summary>
        /// <param name="plugin">插件</param>
        internal void Extension(IPlugin plugin)
        {
            plugin.Name = this.Name;
            plugin.Version = this.Version;
            plugin.Description = this.Description;
            plugin.Status = this.Status;
            plugin.Path = this.Path;
            plugin.Order = this.Order;
            this.plugin = plugin;
        }
    }
    /// <summary>
    /// 插件上下文
    /// </summary>
    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly string _root;
        private readonly AssemblyDependencyResolver _resolver;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="plugin">插件对象</param>
        public PluginLoadContext(IPlugin plugin)
        {
            this._root = Path.GetDirectoryName(plugin.Path);
            this._resolver = new AssemblyDependencyResolver(this._root);
            AssemblyLoadContext.Default.Resolving += Default_Resolving;
        }
        /// <inheritdoc/>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assembly = Default.Assemblies
                                  .FirstOrDefault(s => s.GetName().Name == assemblyName.Name);

            if (assembly != null && assembly.GetName().Version >= assemblyName.Version
                )
            {
                return assembly;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath == null)
            {
                assemblyPath = Path.Combine(this._root, $"{assemblyName.Name}.dll");
            }
            if (File.Exists(assemblyPath))
            {
                assembly = this.LoadFromAssemblyPath(assemblyPath);
            }

            return assembly;
        }
        /// <inheritdoc/>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyPath">程序集路径</param>
        /// <returns>程序集</returns>
        public Assembly Load(string assemblyPath)
        {
            Assembly assembly = null;
            if (File.Exists(assemblyPath))
            {
                assembly = Default.LoadFromAssemblyPath(assemblyPath);
            }
            return assembly;
        }
        /// <summary>
        /// 默认解析器
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集</returns>
        private Assembly Default_Resolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            var assembly = Default.Assemblies
                                  .FirstOrDefault(s => s.GetName().Name == assemblyName.Name);

            if (assembly != null && assembly.GetName().Version >= assemblyName.Version
                )
            {
                return assembly;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath == null)
            {
                assemblyPath = Path.Combine(this._root, $"{assemblyName.Name}.dll");
            }
            if (File.Exists(assemblyPath))
            {
                assembly = Default.LoadFromAssemblyPath(assemblyPath);
            }

            return assembly;
        }
    }
}
