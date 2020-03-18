using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Plugin
{
    public class PluginOptions
    {
        public const string DefaultPath = "plugins";
        public PluginOptions()
        {
            this.path = DefaultPath;
        }

        private string path;

        /// <summary>
        /// 插件目录地址
        /// </summary>
        public string Path
        {
            get => path; set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                this.path = value;
            }
        }
    }
}
