using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Filters
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

        private bool HasAPIResult(ActionExecutedContext context)
        {
            return context.Result is JsonResult || context.Result is ObjectResult;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            

            ErrorOutput errorOutput = null;
            if (context.Exception != null)
            {
                if (context.Exception is SaeException saeException)
                {
                    errorOutput = new ErrorOutput(saeException);
                }
                else
                {
                    errorOutput = new ErrorOutput(context.Exception);
                }
            }

            if (!this.HasAPIResult(context))
            {
                return;
            }

            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value != null &&
                    objectResult.Value is ErrorOutput)
                {
                    errorOutput = (ErrorOutput)objectResult.Value;
                }
                else if (objectResult.Value == null)
                {
                    errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
                }
            }
            else
            {
                var jsonResult = context.Result as JsonResult;
                if (jsonResult.Value != null &&
                    jsonResult.Value is ErrorOutput)
                {
                    errorOutput = (ErrorOutput)jsonResult.Value;
                }
                else
                {
                    errorOutput = new ErrorOutput(StatusCodes.ResourcesNotExist);
                }
            }

            if (errorOutput != null)
            {
                context.Result = new JsonResult(errorOutput);
                context.HttpContext.Response.StatusCode = errorOutput.ToHttpStatusCode();
                context.ExceptionHandled = true;
            }

            //if (context.Result is ObjectResult objectResult)
            //{
            //    this.Wrap(objectResult);
            //}
            //else if (context.Result is JsonResult jsonResult)
            //{
            //    this.Wrap(jsonResult);
            //}
            //else if (context.Result == null)
            //{
            //    var exception = context.Exception;
            //    if (exception == null)
            //    {
            //        context.Result = new ObjectResult(new ResponseResult(StatusCode.Unknown));
            //    }
            //    else
            //    {
            //        if (exception is SaeException saeException)
            //        {
            //            context.Result = new ObjectResult(new ResponseResult(saeException));
            //        }
            //        else
            //        {
            //            context.Result = new ObjectResult(new ResponseResult(exception));
            //        }
            //    }
            //    context.ExceptionHandled = true;
            //}
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        //private void Wrap(dynamic @dynamic)
        //{
        //    ResponseResult response = null;
        //    var result = @dynamic.Value;
        //    if (result == null)
        //    {
        //        response = new ResponseResult(StatusCodes.ResourcesNotExist);
        //    }
        //    else if (!(result is ResponseResult))
        //    {
        //        response = new ResponseResult(result);
        //    }

        //    if (response != null)
        //    {
        //        @dynamic.Value = response;
        //    }
        //}
    }
}
