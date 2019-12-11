using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Builder
{
    /// <summary>
    /// 构建者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBuilder<T> where T : class
    {
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="model"></param>
        void Build(T model);
    }
}
