using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// ABAC授权上下文
    /// </summary>
    public class ABACAuthorizationContext
    {
        /// <summary>
        /// 授权属性
        /// </summary>
        public Dictionary<string, string> Property;
        /// <summary>
        /// 添加授权属性
        /// </summary>
        public virtual void Add(ABACAuthorizationContext context)
        {
            foreach (var p in context.Property)
            {
                this.Add(p.Key, p.Value);
            }
        }
        /// <summary>
        /// 添加属性，相同的属性名将会被替换掉
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Add(string key, string value)
        {
            this.Property[key] = value;
        }
    }
}