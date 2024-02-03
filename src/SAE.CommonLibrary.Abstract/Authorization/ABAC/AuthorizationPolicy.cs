using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 授权策略
    /// </summary>
    public class AuthorizationPolicy
    {
        /// <summary>
        /// 策略标识
        /// </summary>
        /// <value></value>
        public string Id { get; set; }
        /// <summary>
        /// 策略名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 策略的应用规则
        /// </summary>
        /// <value></value>
        public string Rule { get; set; }
    }
}