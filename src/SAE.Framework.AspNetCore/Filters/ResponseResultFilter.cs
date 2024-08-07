﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.AspNetCore.Filters
{
    /// <summary>
    /// 响应筛选器，如果响应存在异常，则使用<see cref="ErrorOutput"/>对异常进行包装。
    /// 否则原样输出。
    /// </summary>
    /// <inheritdoc/>
    public class ResponseResultFilter : IOrderedFilter, IActionFilter
    {
        private readonly ILogging _logging;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志记录器</param>
        public ResponseResultFilter(ILogging<ResponseResultFilter> logging)
        {
            this.Order = FilterScope.Global;
            this._logging = logging;
        }
        /// <inheritdoc/>
        public int Order
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (!context.ExceptionHandled)
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
                    this._logging.Error($"{errorOutput.ToJsonString()}", context.Exception);
                }

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
                }
                else if (context.Result is JsonResult)
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
                    this._logging.Error(errorOutput.ToJsonString());
                }
            }

        }
        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

    }
}
