using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.Framework.Extension
{
    public static partial class UtilityExtension
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>true存在，false不存在</returns>
        public static bool ExistFile(this string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return false;
            }

            return File.Exists(path);
        }
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>true存在，false不存在</returns>
        public static bool ExistDirectory(this string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return false;
            }

            return Directory.Exists(path);
        }
        /// <summary>
        /// 创建目录并返回路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件路径</returns>
        public static string CreateDirectory(this string path)
        {
            if (!path.ExistDirectory())
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
