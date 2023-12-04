using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

[assembly: InternalsVisibleTo("SAE.CommonLibrary.AspNetCore.Test")]

namespace SAE.CommonLibrary.AspNetCore
{
    /// <summary>
    /// 常量集
    /// </summary>
    public partial class Constants
    {
        /// <summary>
        /// cors常量
        /// </summary>
        public class Cors
        {
            /// <summary>
            /// 声明类型
            /// </summary>
            public const string Claim = "cors";
        }
        /// <summary>
        /// Route常量
        /// </summary>
        public class Route
        {
            /// <summary>
            /// 路由列表默认访问路径
            /// </summary>
            public const string DefaultPath = "/.routes";
        }
        /// <summary>
        /// 授权
        /// </summary>
        public class Authorize
        {
            /// <summary>
            /// 配置节根
            /// </summary>
            public const string Option = "auth";
        }
        /// <summary>
        /// 位图授权相关常量
        /// </summary>
        public class BitmapAuthorize
        {
            /// <summary>
            /// 初始化索引
            /// </summary>
            public const int InitialIndex = 1;
            /// <summary>
            /// 权限码最大查找次数
            /// </summary>
            public const int MaxFindNum = 5;
            /// <summary>
            /// 授权声明名词
            /// </summary>
            public const string Claim = "role";
            /// <summary>
            /// 授权组分割符
            /// </summary>
            public const string GroupSeparator = ":";
            /// <summary>
            /// 组格式化形式
            /// </summary>
            /// <value></value>
            public const string GroupFormat = "{0}" + GroupSeparator + "{1}";
            /// <summary>
            /// 授权分割符
            /// </summary>
            public const char Separator = '.';
            /// <summary>
            /// 授权最大为数
            /// </summary>
            public const byte MaxPow = 16;
            /// <summary>
            /// 管理员声明名词
            /// </summary>
            public const string Administrator = "admin";
            /// <summary>
            /// 缓存
            /// </summary>
            public class Caching
            {
                /// <summary>
                /// 授权码
                /// </summary>
                public const string AuthorizeCode = "bitmap_auth_code_";
                /// <summary>
                /// 授权码的缓存时间
                /// </summary>
                public const int AuthorizeCodeTime = 3600 * 24;
            }
        }

        /// <summary>
        /// 字符编码
        /// </summary>
        public static readonly Encoding Encoding = Encoding.Unicode;

    }
}
