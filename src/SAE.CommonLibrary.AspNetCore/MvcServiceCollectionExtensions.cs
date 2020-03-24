using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddResponseResult(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<ResponseResultAttribute>(FilterScope.First);
            });
            return builder;
        }

        public static IApplicationBuilder UseRouteScanning(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetService<IApiDescriptionGroupCollectionProvider>();
            foreach (var group in provider.ApiDescriptionGroups.Items
                                          .SelectMany(group => group.Items)
                                          .Where(s => !s.GetType().IsDefined(typeof(ObsoleteAttribute), false)))
            {
                
            }
            return app;
        }
    }
}
