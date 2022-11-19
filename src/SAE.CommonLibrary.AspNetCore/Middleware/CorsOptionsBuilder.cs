using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Filters
{
    /// <summary>
    /// <see cref="CorsOptions"/>构造器
    /// </summary>
    public class CorsOptionsBuilder
    {
        /// <summary>
        /// 基于用户的cors
        /// </summary>
        /// <param name="options"></param>
        public void UserCorsFilter(CorsOptions options)
        {
            options.AllowRequestAsync += async (HttpContext ctx, string host) =>
            {
                var result = ctx.User?.Identity.IsAuthenticated ?? false;
                if (ctx.User.HasClaim(s => s.Type.Equals(Constants.Cors.Claim)))
                {
                    return ctx.User
                              .FindAll(Constants.Cors.Claim)
                              .Any(s=>host.Equals(s.Value,StringComparison.CurrentCultureIgnoreCase));
                }
                return result;
            };
        }

    }
}
