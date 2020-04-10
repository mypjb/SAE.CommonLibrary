using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore
{
    internal class Constant
    {
        /// <summary>
        /// 默认路由路径
        /// </summary>
        public const string DefaultRoutesPath = "/.routes";
        /// <summary>
        /// 权限位
        /// </summary>
        public const string PermissionBits = "pbits";
        /// <summary>
        /// 权限位分隔符
        /// </summary>
        public const char PermissionBitsSeparator = '.';
        /// <summary>
        /// 权限位最大次幂
        /// </summary>
        public const byte PermissionBitsMaxPow = 7;
   
    }
}
