using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 常量对象
    /// </summary>
    public partial class Constant
    {
        /// <summary>
        /// path constant
        /// </summary>
        public class Path
        {
            /// <summary>
            /// root directory
            /// </summary>
            public static string Root = AppContext.BaseDirectory;
            /// <summary>
            /// config file path
            /// </summary>
            public static string Config = Utils.Path.Root(nameof(Config));
        }
    }
}
