using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class SaeExceptionAttribute : Attribute, IOrderedFilter, IAsyncExceptionFilter
    {
        public SaeExceptionAttribute()
        {
            this.Order = FilterScope.First;
        }
        public int Order { get; set; }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception == null || context.ExceptionHandled)
            {
                return;
            }

            var logging= context.HttpContext.RequestServices.GetService<ILogging<SaeException>>();
            ///记录错误
            logging.Error(context.Exception, context.Exception.Message);
        }
    }
}
