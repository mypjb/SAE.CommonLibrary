using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ResponseResultAttribute : Attribute, IOrderedFilter, IActionFilter
    {
        public ResponseResultAttribute()
        {
            this.Order = FilterScope.First;
        }
        public int Order
        {
            get;
            set;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                this.Wrap(objectResult);
            }
            else if (context.Result is JsonResult jsonResult)
            {
                this.Wrap(jsonResult);
            }
            else if (context.Result == null)
            {
                var exception = context.Exception;
                if (exception == null)
                {
                    context.Result = new ObjectResult(new ResponseResult(StatusCode.Unknown));
                }
                else
                {
                    if (exception is SaeException saeException)
                    {
                        context.Result = new ObjectResult(new ResponseResult(saeException));
                    }
                    else
                    {
                        context.Result = new ObjectResult(new ResponseResult(exception));
                    }
                }
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        private void Wrap(dynamic @dynamic)
        {
            ResponseResult response = null;
            var result = @dynamic.Value;
            if (result == null)
            {
                response = new ResponseResult(StatusCode.ResourcesNotExist);
            }
            else if (!(result is ResponseResult))
            {
                response = new ResponseResult(result);
            }

            if (response != null)
            {
                @dynamic.Value = response;
            }
        }
    }
}
