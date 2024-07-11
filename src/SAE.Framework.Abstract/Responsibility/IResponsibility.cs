using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Responsibility
{
    /// <summary>
    /// 职责链接口
    /// </summary>
    /// <typeparam name="TContext">上下文类型</typeparam>
    public interface IResponsibility<TContext> where TContext : ResponsibilityContext
    {
        /// <summary>
        /// 执行职责链
        /// </summary>
        /// <param name="context">上下文</param>
        Task HandleAsync(TContext context);
    }
}
