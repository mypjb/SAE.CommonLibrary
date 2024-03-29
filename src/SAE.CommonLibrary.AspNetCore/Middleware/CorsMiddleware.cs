﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<CorsOptions> _options;
        private readonly ILogging _logging;
        private readonly Regex _hostRegex;
        public CorsMiddleware(RequestDelegate next,
                              IOptions<CorsOptions> options,
                              ILogging<CorsMiddleware> logging)
        {
            this._next = next;
            this._options = options;
            this._logging = logging;
            this._hostRegex = new Regex("https?://[\\w\\.:]+");
        }


        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            StringValues origin;
            if (request.Headers.TryGetValue(HeaderNames.Origin, out origin) &&
                !origin.FirstOrDefault().IsNullOrWhiteSpace())
            {
                var originHost = origin.First();

                var options = this._options.Value;

                this._logging.Debug($"Cors request origin:{origin}");

                if (await options.AllowRequestAsync(context, originHost))
                {
                    context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowOrigin, originHost);

                    if (request.Headers.ContainsKey(HeaderNames.AccessControlRequestHeaders))
                    {
                        context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowHeaders, request.Headers[HeaderNames.AccessControlRequestHeaders]);
                    }else if (!options.AllowHeaders.IsNullOrWhiteSpace())
                    {
                        context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowHeaders, options.AllowHeaders);
                    }

                    if (request.Headers.ContainsKey(HeaderNames.AccessControlRequestMethod))
                    {
                        context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowMethods, request.Headers[HeaderNames.AccessControlRequestMethod]);
                    }else if (!options.AllowMethods.IsNullOrWhiteSpace())
                    {
                        context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowMethods, options.AllowMethods);
                    }

                    if (!options.AllowCredentials.IsNullOrWhiteSpace())
                    {
                        context.Response.Headers.TryAdd(HeaderNames.AccessControlAllowCredentials, options.AllowCredentials);
                    }

                    if (request.Method.Equals(HttpMethod.Options.Method, StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        return;
                    }
                }
                else
                {
                    this._logging.Error($"Reject cross-domain requests");
                }
            }
            else
            {
                this._logging.Debug($"This not cors request {request.Path}");
            }

            await this._next.Invoke(context);
        }
    }
}
