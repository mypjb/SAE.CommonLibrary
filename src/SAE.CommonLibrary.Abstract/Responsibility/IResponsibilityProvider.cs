using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    /// <summary>
    /// 职责链提供程序
    /// </summary>
    /// <typeparam name="TResponsibilityContext"></typeparam>
    /// <remarks>
    /// 该接口会将相关链条进行组合
    /// </remarks>
    public interface IResponsibilityProvider<TResponsibilityContext> where TResponsibilityContext : ResponsibilityContext
    {
        /// <summary>
        /// 链根
        /// </summary>
        IResponsibility<TResponsibilityContext> Root { get; }
        /// <summary>
        /// 链条列表
        /// </summary>
        IEnumerable<IResponsibility<TResponsibilityContext>> Responsibilities { get;  }
    }
}
