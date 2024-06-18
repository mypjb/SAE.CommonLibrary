using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Plugin
{
    /// <summary>
    /// 插件类型
    /// </summary>
    public class PluginOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = "plugin";
        /// <summary>
        /// ctor
        /// </summary>
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
