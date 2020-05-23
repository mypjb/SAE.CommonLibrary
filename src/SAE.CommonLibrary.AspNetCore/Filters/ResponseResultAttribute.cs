﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

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

        public void OnActionExecuted(ActionExecutedContext context)
        {
            int statusCode = (int)HttpStatusCode.OK;
            if (context.Exception != null)
            {
                if(context.Exception is SaeException)
                {
                    statusCode = (int)((SaeException)context.Exception).Code;
                    
                }
                else
                {
                    statusCode= (int)HttpStatusCode.InternalServerError; 
                }
            }
            else
            {
                if (context.Result is ObjectResult objectResult)
                {
                    
                }
                else if (context.Result is JsonResult jsonResult)
                {
                    
                }
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
