using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// <param name="fileFullName"></param>
        /// <returns></returns>
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
        /// <param name="path"></param>
        /// <returns></returns>
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
