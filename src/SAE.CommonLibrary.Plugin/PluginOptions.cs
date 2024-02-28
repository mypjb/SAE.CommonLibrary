using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Plugin
{
    public class PluginOptions
    {
        public const string Option = "plugin";
        public PluginOptions()
        {
        }

        private string path;

        /// <summary>
        /// 插件目录地址
        /// </summary>
        public string Path
        {
            get; set;
        }
    }
}
