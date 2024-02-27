using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace SAE.CommonLibrary.Plugin
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 所在路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
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
