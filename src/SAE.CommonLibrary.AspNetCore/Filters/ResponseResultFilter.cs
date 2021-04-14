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
            this.Order = FilterScope.First;
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

            //if (context.Result is ObjectResult objectResult)
            //{
            //    if (objectResult.Value != null &&
            //        objectResult.Value is ErrorOutput)
            //    {
            //        errorOutput = (ErrorOutput)objectResult.Value;
            //    }
            //    else if (objectResult.Value == null)
            //    {
            //        errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
            //    }
            //}
            //else if (context.Result is JsonResult)
            //{
            //    var jsonResult = context.Result as JsonResult;
            //    if (jsonResult.Value != null &&
            //        jsonResult.Value is ErrorOutput)
            //    {
            //        errorOutput = (ErrorOutput)jsonResult.Value;
            //    }
            //    else if (jsonResult.Value == null)
            //    {
            //        errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
            //    }
            //}
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

    }
}
