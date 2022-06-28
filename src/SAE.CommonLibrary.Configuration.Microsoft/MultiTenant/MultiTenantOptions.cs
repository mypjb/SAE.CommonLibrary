using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    /// <summary>
    /// multi tenant options
    /// </summary>
    public class MultiTenantOptions<TOptions> where TOptions : class
    {
        public MultiTenantOptions()
        {
            this.ConfigurationNodeName = SAE.CommonLibrary.Constant.Scope;
        }
        /// <summary>
        /// configuration node name
        /// </summary>
        /// <value></value>
        public string ConfigurationNodeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Key {get;set;}

    }
}