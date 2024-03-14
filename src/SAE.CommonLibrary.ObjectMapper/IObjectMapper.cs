using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.ObjectMapper
{
    /// <summary>
    /// 对象映射接口
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        IObjectMapper Bind(Type sourceType, Type targetType);
        /// <summary>
        /// bind是否存在
        /// </summary>
        /// <param name="sourceType">源</param>
        /// <param name="targetType">目标</param>
        /// <returns>true:存在</returns>
        bool BindingExists(Type sourceType, Type targetType);
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="source">源</param>
        /// <param name="target">目标，如果目标已存在，则采用附加的形式</param>
        /// <returns>映射后的对象</returns>
        object Map(Type sourceType, Type targetType, object source, object target = null);
    }
}
