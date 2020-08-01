﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("SAE.CommonLibrary.AspNetCore.Test")]

namespace SAE.CommonLibrary.AspNetCore
{
    public class Constant
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
        public const byte PermissionBitsMaxPow = 8;
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string Administrator = "admin";
        public const string OptionName = "authorize";
    }
}
