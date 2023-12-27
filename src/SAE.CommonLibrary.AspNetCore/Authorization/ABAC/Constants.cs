using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore
{
    public partial class Constants
    {
        public class Request
        {
            /// <summary>
            /// 结尾符
            /// </summary>
            public const char EndingSymbol = '/';
        }
        /// <summary>
        /// ABAC 授权
        /// </summary>
        public class ABAC
        {
            /// <summary>
            /// 请求
            /// </summary>
            public const string Request = "request";
            /// <summary>
            /// 客户端
            /// </summary>
            public const string Client = "client";
            /// <summary>
            /// 服务端
            /// </summary>
            public const string Server = "server";

            /// <summary>
            /// 用户
            /// </summary>
            public const string User = "user";
            /// <summary>
            /// 应用名称
            /// </summary>
            public const string AppName = Server + ".app_name";
            /// <summary>
            /// 认证通过属性
            /// </summary>
            public const string Authentication = User + ".auth";
            /// <summary>
            /// 请求路径
            /// </summary>
            public const string Path = Request + ".path";
            /// <summary>
            /// 环境变量
            /// </summary>
            public const string Environment = Server + ".env";
            /// <summary>
            /// 时间戳
            /// </summary>
            public const string Timestamp = Server + ".timestamp";
            /// <summary>
            /// 客户端IP
            /// </summary>
            public const string ClientIP = Client + ".ip";
            /// <summary>
            /// 服务端IP
            /// </summary>
            public const string ServerIP = Server + ".ip";
            /// <summary>
            /// 协议
            /// </summary>
            public const string Scheme = Request + ".scheme";
            /// <summary>
            /// 设备
            /// </summary>
            public const string Device = Client + ".device";
            /// <summary>
            /// 主机地址
            /// </summary>
            public const string Host = Request + ".host";

            /// <summary>
            /// 端口
            /// </summary>
            public const string Port = Request + ".port";
            /// <summary>
            /// 位置
            /// </summary>
            public const string Location = Client + ".location";
        }
    }
}