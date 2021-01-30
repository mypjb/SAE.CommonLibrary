using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    public class Constants
    {
        /// <summary>
        /// 配置根目录
        /// </summary>
        public const string ConfigRootDirectoryKey = "SAE:CONFIG:ROOT";
        /// <summary>
        /// 默认的根配置目录
        /// </summary>
        public const string DefaultConfigRootDirectory = "Config";
        /// <summary>
        /// 配置节点
        /// </summary>
        public const string ConfigNodeKey = "SAE:CONFIG";

        public const string FileSeparator = ".";
        public const char ConfigSeparator = ':';
        public const string JsonSuffix = ".json";
        public const int DefaultPollInterval = 60;
    }
}
