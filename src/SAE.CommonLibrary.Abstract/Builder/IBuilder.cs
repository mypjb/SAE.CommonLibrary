using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Builder
{
    /// <summary>
    /// 该接口用于标记“构建者”,继承构建接口请使用<see cref="IBuilder{T}"/>
    /// </summary>
    [Obsolete("该接口用于标记“构建者”,继承构建接口请使用IBuilder<T>")]
    public interface IBuilder
    {

    }
    /// <summary>
    /// 构建者
    /// </summary>
    /// <typeparam name="T">构建类型</typeparam>
    public interface IBuilder<T> : IBuilder where T : class
    {
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="model">构建对象</param>
        Task BuildAsync(T model);
    }
}
