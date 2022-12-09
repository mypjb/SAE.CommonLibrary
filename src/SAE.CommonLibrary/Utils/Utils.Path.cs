using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 工具类
    /// </summary>
    public partial class Utils
    {
        /// <summary>
        /// 路径组合
        /// </summary>
        public class Path
        {
            /// <summary>
            /// 将<paramref name="paths"/>和<seealso cref="Constants.Path.Config"/>组合成路径
            /// </summary>
            /// <param name="paths">路径列表</param>
            /// <returns></returns>
            public static string Config(params string[] paths)
            {
                List<string> pathList = new List<string>();

                pathList.Add(Constants.Path.Config);
                if (paths != null)
                {
                    pathList.AddRange(paths);
                }

                return System.IO.Path.Combine(pathList.ToArray());
            }
            /// <summary>
            /// 将<paramref name="paths"/>和<seealso cref="Constants.Path."/>组合成路径
            /// </summary>
            /// <param name="paths">路径列表</param>
            /// <returns></returns>
            public static string Root(params string[] paths)
            {
                List<string> pathList = new List<string>();

                pathList.Add(Constants.Path.Root);
                if (paths != null)
                {
                    pathList.AddRange(paths);
                }

                return System.IO.Path.Combine(pathList.ToArray());
            }
        }
    }
}
