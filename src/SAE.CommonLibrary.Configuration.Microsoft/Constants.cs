using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    public class Constants
    {
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

       
        public const string FileSeparator = ".";
        public const char ConfigSeparator = ':';
        public const string JsonSuffix = ".json";
        public const int DefaultPollInterval = 60;
        public const int DefaultClientTimeout = 1000 * 3;
    }
}
