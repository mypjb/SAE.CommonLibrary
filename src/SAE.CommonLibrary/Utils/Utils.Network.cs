using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace SAE.CommonLibrary
{
    public partial class Utils
    {
        /// <summary>
        /// 网络
        /// </summary>
        public class Network
        {

            /// <summary>
            /// 获得服务端地址
            /// </summary>
            public static string GetServerIP()
            {
                var networks = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var network in networks)
                {
                    var address = network.GetIPProperties()
                                        .UnicastAddresses
                                        .Where(s => (s.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                                                    s.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) &&
                                                    !IPAddress.IsLoopback(s.Address))
                                        .FirstOrDefault()
                                        ?.Address;

                    if (address != null)
                    {
                        return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 ?
                                                        address.MapToIPv6().ToString() :
                                                        address.MapToIPv4().ToString();
                    }
                }
                return IPAddress.Loopback.ToString();
            }

            /// <summary>
            /// 判断IP地址是否为内网IP地址
            /// </summary>
            /// <param name="ipAddress">IP地址字符串</param>
            /// <returns>true:内网，false:外网</returns>
            public static bool IsInnerIP(string ipAddress)
            {
                bool isInnerIp = false;
                long ipNum = GetIpNum(ipAddress);
                /***
                私有IP：A类 10.0.0.0-10.255.255.255
                B类 172.16.0.0-172.31.255.255
                C类 192.168.0.0-192.168.255.255
                当然，还有127这个网段是环回地址
                ***/
                long aBegin = GetIpNum("10.0.0.0");
                long aEnd = GetIpNum("10.255.255.255");
                long bBegin = GetIpNum("172.16.0.0");
                long bEnd = GetIpNum("172.31.255.255");
                long cBegin = GetIpNum("192.168.0.0");
                long cEnd = GetIpNum("192.168.255.255");
                isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd) || ipAddress.Equals("127.0.0.1");
                return isInnerIp;
            }
            /// <summary>
            /// 把IP地址转换为Long型数字
            /// </summary>
            /// <param name="ipAddress">IP地址字符串</param>
            /// <returns>long类型的IP地址</returns>
            private static long GetIpNum(string ipAddress)
            {
                String[] ip = ipAddress.Split('.');
                long a = int.Parse(ip[0]);
                long b = int.Parse(ip[1]);
                long c = int.Parse(ip[2]);
                long d = int.Parse(ip[3]);

                long ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
                return ipNum;
            }
            /// <summary>
            /// 判断用户IP地址转换为Long型后是否在内网IP地址所在范围
            /// </summary>
            /// <param name="userIp">用户ip</param>
            /// <param name="begin">开始范围</param>
            /// <param name="end">结束范围</param>
            /// <returns>true:内网,false:外网</returns>
            private static bool IsInner(long userIp, long begin, long end)
            {
                return (userIp >= begin) && (userIp <= end);
            }
        }

    }
}
