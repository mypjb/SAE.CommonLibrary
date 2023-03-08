using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// 配置源的常量
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 根目录key
            /// </summary>
            public const string RootDirectoryKey = "SAE:CONFIG:ROOT";

            /// <summary>
            /// 包含远程的配置节
            /// </summary>
            public const string IncludeEndpointConfiguration= "SAE:CONFIG:INCLUDE";
            /// <summary>
            /// 默认根目录
            /// </summary>
            public const string DefaultRootDirectory = "Config";
            /// <summary>
            /// 配置key
            /// </summary>
            public const string OptionKey = "SAE:CONFIG";
        }

        /// <summary>
        /// 文件分割符
        /// </summary>
        public const string FileSeparator = ".";
        /// <summary>
        /// 配置分割符
        /// </summary>
        public const char ConfigSeparator = ':';
        /// <summary>
        /// 配置节分割符
        /// </summary>
        public const char ConfigurationSectionSeparator = '.';
        /// <summary>
        /// json后缀
        /// </summary>
        public const string JsonSuffix = ".json";
        /// <summary>
        /// 配置文件默认轮询时间
        /// </summary>
        public const int DefaultPollInterval = 60;
        /// <summary>
        /// 客户端默认超时时间
        /// </summary>
        public const int DefaultClientTimeout = 1000 * 3;
        /// <summary>
        /// 下一次请求头默认名称
        /// </summary>
        public const string DefaultNextRequestHeaderName = "Config-Next";
    }
}
