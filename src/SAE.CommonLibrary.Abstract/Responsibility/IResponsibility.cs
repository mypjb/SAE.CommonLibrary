using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public interface IResponsibility<TContext>where TContext:ResponsibilityContext
    {
        /// <summary>
        /// 执行职责链
        /// </summary>
        /// <param name="context"></param>
        Task HandleAsync(TContext context);
    }
}
