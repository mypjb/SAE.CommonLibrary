using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Database
{
    public class DBConnectOptions
    {
        /// <summary>
        /// connection name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// database provider
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// connection string
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
