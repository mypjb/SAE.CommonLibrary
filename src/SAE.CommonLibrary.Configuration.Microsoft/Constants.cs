using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// 
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

            /// <summary>
            /// system key
            /// </summary>
            public const string SystemKey = "System:Id";
        }

        /// <summary>
        /// 
        /// </summary>
        public const string FileSeparator = ".";
        /// <summary>
        /// 
        /// </summary>
        public const char ConfigSeparator = ':';
        /// <summary>
        /// 
        /// </summary>
        public const string JsonSuffix = ".json";
        /// <summary>
        /// 
        /// </summary>
        public const int DefaultPollInterval = 60;
        /// <summary>
        /// 
        /// </summary>
        public const int DefaultClientTimeout = 1000 * 3;
    }
}
