using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore
{
    public partial class Constants
    {
        /// <summary>
        /// ABAC 授权
        /// </summary>
        public class ABAC
        {
            /// <summary>
            /// 请求路径
            /// </summary>
            public const string Path = "path";
            /// <summary>
            /// 环境变量
            /// </summary>
            public const string Environment = "env";
            /// <summary>
            /// 时间戳
            /// </summary>
            public const string Timestamp = "timestamp";
            /// <summary>
            /// 客户端IP
            /// </summary>
            public const string ClientIP = "client_ip";
            /// <summary>
            /// 服务端IP
            /// </summary>
            public const string ServerIP = "server_ip";
            /// <summary>
            /// 协议
            /// </summary>
            public const string Scheme = "scheme";
            /// <summary>
            /// 设备
            /// </summary>
            public const string Device = "device";
            /// <summary>
            /// 主机地址
            /// </summary>
            public const string Host = "host";

            /// <summary>
            /// 端口
            /// </summary>
            public const string Port = "port";
            /// <summary>
            /// 位置
            /// </summary>
            public const string Location = "location";
        }
    }
}