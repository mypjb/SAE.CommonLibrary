using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.Framework
{

    public partial class Constants
    {
        /// <summary>
        /// 路径常量
        /// </summary>
        public class Path
        {
            /// <summary>
            /// 根目录
            /// </summary>
            public static string Root = AppContext.BaseDirectory;
            /// <summary>
            /// 配置文件路径
            /// </summary>
            public static string Config = Utils.Path.Root(nameof(Config));
        }
    }
}
