using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    /// <summary>
    /// 异常筛选器，这里只记录错误，不作其他处理。
    /// </summary>
    public class SAEExceptionFilter : IOrderedFilter, IAsyncExceptionFilter
    {
        private readonly ILogging<SAEException> _logging;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志记录器</param>
        public SAEExceptionFilter(ILogging<SAEException> logging)
        {
            this.Order = FilterScope.First;
            this._logging = logging;
        }
        /// <inheritdoc/>
        public int Order { get; set; }
        /// <inheritdoc/>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception == null || context.ExceptionHandled)
            {
                this._logging.Error(context.Exception, "异常已被处理");
                return Task.CompletedTask;
            }
            this._logging.Error(context.Exception, context.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
