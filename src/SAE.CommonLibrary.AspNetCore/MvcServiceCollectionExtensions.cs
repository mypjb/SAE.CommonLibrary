using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddResponseResult(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options=>
            {
                options.Filters.Add<ResponseResultAttribute>(FilterScope.First);
            });
            return builder;
        } 
    }
}
