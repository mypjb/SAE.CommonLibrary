using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SAE.CommonLibrary.AspNetCore.Test")]

namespace SAE.CommonLibrary.AspNetCore
{
    public class Constants
    {
        /// <summary>
        /// Claim type
        /// </summary>
        public const string CorsClaim = "cors";
        /// <summary>
        /// Cors Claim 分隔符
        /// </summary>
        public const char CorsSeparator = ';';
        /// <summary>
        /// 默认路由路径
        /// </summary>
        public const string DefaultRoutesPath = "/.routes";
        /// <summary>
        /// 权限位
        /// </summary>
        public const string PermissionBits = "pbits";
        /// <summary>
        /// 权限位，组分隔符
        /// </summary>
        public const string PermissionGroupSeparator = ":";
        /// <summary>
        /// 权限位分隔符
        /// </summary>
        public const char PermissionBitsSeparator = '.';
        /// <summary>
        /// 权限位最大次幂
        /// </summary>
        public const byte PermissionBitsMaxPow = 16;
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string Administrator = "admin";
        /// <summary>
        /// 字符编码
        /// </summary>
        public static readonly Encoding Encoding = Encoding.Unicode;

    }
}
