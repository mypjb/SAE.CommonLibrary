using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.CommonLibrary
{
    
    /// <summary>
    /// system constant
    /// </summary>
    public partial class Constant
    {
        /// <summary>
        /// default encoding
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// time zone generator
        /// </summary>
        public static Func<DateTime> TimeZoneGenerator = () => DateTime.UtcNow;

        /// <summary>
        /// default name
        /// </summary>
        public const string Default = "";
        /// <summary>
        /// scope default configuration scope
        /// </summary>
        public const string Scope = nameof(Scope);
        /// <summary>
        /// default Time zone utc
        /// </summary>
        public static DateTime DefaultTimeZone
        {
            get => TimeZoneGenerator.Invoke();
        }
    }
}
