using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// configuration Constants
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// config const
        /// </summary>
        public class Config
        {
            /// <summary>
            /// root directory key
            /// </summary>
            public const string RootDirectoryKey = "SAE:CONFIG:ROOT";
            /// <summary>
            /// default root directory
            /// </summary>
            public const string DefaultRootDirectory = "Config";
            /// <summary>
            /// option key
            /// </summary>
            public const string OptionKey = "SAE:CONFIG";
        }

        /// <summary>
        /// file separator 
        /// </summary>
        public const string FileSeparator = ".";
        /// <summary>
        /// config separator
        /// </summary>
        public const char ConfigSeparator = ':';
        /// <summary>
        /// configuraion section separator
        /// </summary>
        public const char ConfiguraionSectionSeparator = '.';
        /// <summary>
        /// json suffix
        /// </summary>
        public const string JsonSuffix = ".json";
        /// <summary>
        /// default poll interval
        /// </summary>
        public const int DefaultPollInterval = 60;
        /// <summary>
        /// default client timeout
        /// </summary>
        public const int DefaultClientTimeout = 1000 * 3;
        /// <summary>
        /// default next request header name
        /// </summary>
        public const string DefaultNextRequestHeaderName = "Config-Next";
    }
}
