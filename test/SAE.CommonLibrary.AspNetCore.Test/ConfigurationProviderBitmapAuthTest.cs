using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class ConfigurationProviderBitmapAuthTest : BitmapAuthTest
    {
        public ConfigurationProviderBitmapAuthTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void AddProvider(BitmapAuthorizationBuilder builder)
        {
            builder.AddConfigurationProvider();
        }
    }
}
