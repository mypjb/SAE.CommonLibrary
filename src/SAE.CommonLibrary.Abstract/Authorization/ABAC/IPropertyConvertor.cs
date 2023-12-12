using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 属性转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyConvertor<T>
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="val"></param>
        T Convert(string val);
    }
}