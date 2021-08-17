using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SAE.CommonLibrary.AspNetCore.Test")]

namespace SAE.CommonLibrary.AspNetCore
{
    public class Constants
    {
        public class Cors
        {
            /// <summary>
            /// claim type
            /// </summary>
            public const string Claim = "cors";
            /// <summary>
            /// cors claim separator
            /// </summary>
            public const char Separator = ';';
        }
       
        public class Route
        {
            /// <summary>
            /// default path
            /// </summary>
            public const string DefaultPath = "/.routes";
        }
        
        public class BitmapAuthorize
        {
            /// <summary>
            /// permission bit initial index
            /// </summary>
            public const int InitialIndex = 1;
            /// <summary>
            /// permission bit claim name
            /// </summary>
            public const string Claim = "pbits";
            /// <summary>
            /// permission bit format string
            /// </summary>
            public const string Format = "{0}" + GroupSeparator + "{1}";
            /// <summary>
            /// permission bit group separator
            /// </summary>
            public const string GroupSeparator = ":";
            /// <summary>
            /// permission bit separator
            /// </summary>
            public const char Separator = '.';
            /// <summary>
            /// permission bit max pow
            /// </summary>
            public const byte MaxPow = 16;
            /// <summary>
            /// administrator claim name
            /// </summary>
            public const string Administrator = "admin";
        }
       
        /// <summary>
        /// 字符编码
        /// </summary>
        public static readonly Encoding Encoding = Encoding.Unicode;

    }
}
