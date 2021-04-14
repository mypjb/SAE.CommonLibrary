using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    public class CorsOptionsBuilder
    {
        public void UserCorsFilter(CorsOptions options)
        {
            options.AllowRequestAsync += async (HttpContext ctx, string host) =>
            {
                var result = ctx.User?.Identity.IsAuthenticated ?? false;
                if (ctx.User.HasClaim(s => s.Type.Equals(Constants.CorsClaim)))
                {
                    return ctx.User
                              .FindFirst(Constants.CorsClaim)
                              .Value
                              .Split(Constants.CorsSeparator)
                              .Contains(host, StringComparer.OrdinalIgnoreCase);
                }
                return result;
            };
        }

    }
}
