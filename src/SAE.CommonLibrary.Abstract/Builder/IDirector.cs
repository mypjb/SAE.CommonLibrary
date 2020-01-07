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
        Task Build<T>(T model) where T : class;
    }
    /// <summary>
    /// 指挥者具体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDirector<T> where T : class
    {
        Task Build(T model);
    }
    
}
