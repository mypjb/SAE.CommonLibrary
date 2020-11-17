using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace SAE.CommonLibrary.Plugin
{
    public interface IPlugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
    }


    public class Plugin : IPlugin
    {
        public Plugin()
        {
            this.Status = true;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public bool Status { get; set; }
        public int Order { get; set; }
    }
}
