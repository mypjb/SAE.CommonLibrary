using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Builder
{
    /// <summary>
    /// 指挥者代理接口
    /// </summary>
    public interface IDirector
    {
        /// <summary>
        /// 开始构建对象
        /// </summary>
        /// <typeparam name="T">构建的类型</typeparam>
        /// <param name="model">构建的对象</param>
        Task BuildAsync<T>(T model) where T : class;
    }
    /// <summary>
    /// 指挥者具体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDirector<T> where T : class
    {
        /// <summary>
        /// 开始构建对象
        /// </summary>
        /// <param name="model">构建的对象</param>
        Task BuildAsync(T model);
    }

}
