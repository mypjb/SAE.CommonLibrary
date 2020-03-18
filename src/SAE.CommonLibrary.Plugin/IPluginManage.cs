using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Plugin
{
    public interface IPluginManage
    {
        IEnumerable<IPlugin> Plugins { get; }
        Assembly Load(IPlugin plugin);
        void UnLoad(string name);
    }
}
