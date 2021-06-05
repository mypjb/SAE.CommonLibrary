using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class ResponseResultFilter : IOrderedFilter, IActionFilter
    {
        public ResponseResultFilter()
        {
            this.Order = FilterScope.Global;
        }
        public int Order
        {
            get;
            set;
        }

        private bool HasAPIResult(ActionExecutedContext context)
        {
            return context.Result is JsonResult || context.Result is ObjectResult;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                ErrorOutput errorOutput;
                if (context.Exception is SAEException saeException)
                {
                    errorOutput = new ErrorOutput(saeException);
                }
                else
                {
                    errorOutput = new ErrorOutput(context.Exception);
                }
                context.Result = new JsonResult(errorOutput);
                context.HttpContext.Response.StatusCode = errorOutput.ToHttpStatusCode();
                context.ExceptionHandled = true;
            }
            else
            {
                ErrorOutput errorOutput = null;
                if (context.Result is ObjectResult objectResult)
                {
                    if (objectResult.Value == null)
                    {
                        errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
                    }
                }else if (context.Result is JsonResult)
                {
                    var jsonResult = context.Result as JsonResult;
                    if (jsonResult.Value == null)
                    {
                        errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
                    }
                }

                if (errorOutput != null)
                {
                    context.Result = new JsonResult(errorOutput);
                    context.HttpContext.Response.StatusCode = errorOutput.ToHttpStatusCode();
                }
            }


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

    }
}
