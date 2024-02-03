using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 授权描述符
    /// </summary>
    public class AuthDescriptor
    {
        /// <summary>
        /// 描述符的唯一Key
        /// </summary>
        /// <value></value>
        public virtual string Key { get; set; }
        /// <summary>
        /// 授权描述
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// 策略标识集合
        /// </summary>
        public string[] PolicyIds { get; set; }
    }
}