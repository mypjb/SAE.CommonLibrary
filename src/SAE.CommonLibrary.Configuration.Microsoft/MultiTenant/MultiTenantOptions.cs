using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    /// <summary>
    /// 多租户配置
    /// </summary>
    public class MultiTenantOptions
    {
        /// <summary>
        /// 配置key
        /// </summary>
        public const string Options = "multitenant:options";
    }
    /// <summary>
    /// 多租户配置
    /// </summary>
    public class MultiTenantOptions<TOptions> where TOptions : class
    {

        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        public MultiTenantOptions()
        {
            this.ConfigurationNodeName = SAE.CommonLibrary.Constants.Scope;
        }
        /// <summary>
        /// 配置节点的名称
        /// </summary>
        /// <value></value>
        public string ConfigurationNodeName { get; set; }
        /// <summary>
        /// 配置Key
        /// </summary>
        /// <value></value>
        public string Key { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

    }
}