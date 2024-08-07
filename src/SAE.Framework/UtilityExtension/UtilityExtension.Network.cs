﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SAE.Framework.Extension
{
    public static partial class UtilityExtension
    {
        /// <summary>
        /// 判断<paramref name="address"/>是否为内网Ip
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <returns>true是内网，false不是内网</returns>
        public static bool IsInnerIP(this IPAddress address)
        {
            return Utils.Network.IsInnerIP(address.MapToIPv4().ToString());
        }
    }
}
