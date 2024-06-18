using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 属性转换器
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    public interface IPropertyConvertor<T>
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="val">属性的字符形式</param>
        /// <returns>属性对象</returns>
        T Convert(string val);
    }
}