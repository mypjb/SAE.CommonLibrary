using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        /// <summary>
        /// 判断<paramref name="address"/>是否为内网Ip
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsInnerIP(this IPAddress address)
        {
            return Utils.Network.IsInnerIP(address.MapToIPv4().ToString());
        }
    }
}
