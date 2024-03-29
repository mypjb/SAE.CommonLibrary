﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class CorsOptions
    {
        public const string Options = "cors";
        public CorsOptions()
        {
            this.AllowHeaders = "*";
            this.AllowMethods = "GET, HEAD, POST, PUT, DELETE, CONNECT, OPTIONS, TRACE";
            this.AllowCredentials = "true";
        }
        /// <summary>
        /// 允许Header 默认*
        /// </summary>
        public string AllowHeaders { get; set; }
        /// <summary>
        /// 允许Methods 默认 GET, HEAD, POST, PUT, DELETE, CONNECT, OPTIONS, TRACE
        /// </summary>
        public string AllowMethods { get; set; }
        /// <summary>
        /// 允许 Credentials 默认 true
        /// </summary>
        public string AllowCredentials { get; set; }

        public Func<HttpContext, string, Task<bool>> AllowRequestAsync = (ctx, url) => Task.FromResult(true);
    }
}
