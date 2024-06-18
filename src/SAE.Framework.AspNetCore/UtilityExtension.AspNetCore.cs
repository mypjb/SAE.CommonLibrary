using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Extension
{
    /// <summary>
    /// 扩展程序
    /// </summary>
    public static class UtilityExtension
    {
        /// <summary>
        /// 请求是否来自于ajax
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers[HeaderNames.XRequestedWith] == "XMLHttpRequest";
            return false;
        }

        /// <summary>
        /// 获得客户端IP
        /// </summary>
        public static string GetClientIP(this HttpRequest request)
        {
            var ip = string.Empty;
            if (request.Headers.TryGetValue("X-Real-IP", out var realIP))
            {
                ip = realIP.ToString();
            }
            else if (request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
            {
                ip = forwarded.ToString()
                              .Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .First();
            }
            else
            {
                ip = IPAddress.Loopback.ToString();;
            }

            return ip;
        }

        
    }
}
