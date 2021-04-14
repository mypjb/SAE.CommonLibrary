using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class SaeExceptionFilter : IOrderedFilter, IAsyncExceptionFilter
    {
        private readonly ILogging<SAEException> _logging;

        public SaeExceptionFilter(ILogging<SAEException> logging)
        {
            this.Order = FilterScope.First;
            this._logging = logging;
        }
        public int Order { get; set; }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception == null || context.ExceptionHandled)
            {
                return Task.CompletedTask;
            }

            ///记录错误
            this._logging.Error(context.Exception, context.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
