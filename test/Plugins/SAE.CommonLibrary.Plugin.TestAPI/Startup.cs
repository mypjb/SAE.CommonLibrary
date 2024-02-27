using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAE.CommonLibrary.Plugin.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SAE.CommonLibrary.Plugin.TestAPI
{
    public class Startup : WebPlugin
    {


        public override void PluginConfigureServices(IServiceCollection services)
        {
            
        }

        public override void PluginConfigure(IApplicationBuilder app)
        {
            
        }
    }
}
