using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace SAE.CommonLibrary.Plugin
{
    internal class ProxyPlugin : IPlugin
    {
        internal IPlugin plugin;
        private readonly Lazy<PluginLoadContext> _loadContext;

        public ProxyPlugin(IPlugin plugin)
        {
            this.plugin = plugin;
            if(this.plugin is ProxyPlugin)
            {
                _loadContext = (this.plugin as ProxyPlugin)._loadContext;
            }
            else
            {
                _loadContext = new Lazy<PluginLoadContext>(InitContext);
            }
            
        }

        private PluginLoadContext InitContext()
        {
            return new PluginLoadContext(this);
        }

        public string Name { get => plugin.Name; set => plugin.Name = value; }
        public string Description { get => plugin.Description; set => plugin.Description = value; }
        public string Version { get => plugin.Version; set => plugin.Version = value; }
        public string Path { get => plugin.Path; set => plugin.Path = value; }
        public bool Status { get => plugin.Status; set => plugin.Status = value; }

        internal Assembly Register()
        {
            return this._loadContext.Value.LoadFromAssemblyPath(this.Path);
        }

        internal void Delete()
        {
            this._loadContext.Value.Unload();
        }

        internal bool Check()
        {

            return !(string.IsNullOrWhiteSpace(this.Name) ||
                     string.IsNullOrWhiteSpace(this.Version) ||
                     string.IsNullOrWhiteSpace(this.Path));
        }

        internal void Extension(IPlugin plugin)
        {
            plugin.Name = this.Name;
            plugin.Version = this.Version;
            plugin.Description = this.Version;
            plugin.Status = this.Status;
            plugin.Path = this.Path;
            this.plugin = plugin;
        }
    }

    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly string _root;
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(IPlugin plugin)
        {
            this._root = Path.GetDirectoryName(plugin.Path);
            this._resolver = new AssemblyDependencyResolver(this._root);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assembly = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .FirstOrDefault(s => s.GetName().Name == assemblyName.Name);

            if (assembly != null && assembly.GetName().Version >= assemblyName.Version)
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

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
