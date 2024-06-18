using System;

namespace SAE.Framework.Scope
{
    /// <summary>
    /// 区域标识接口
    /// </summary>
    public interface IScope:IDisposable
    {
        /// <summary>
        /// 区域的名称
        /// </summary>
        string Name { get;}
    }
}