using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Xunit;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test
{
    public class MicrosoftConfigurationExtensionsTest
    {
        public const string ApplicationiName = nameof(ApplicationiName);
        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void JsonFileDirectory(string env)
        {
            var root = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()
            {
                { HostDefaults.EnvironmentKey,env }
            }).AddJsonFileDirectory().Build();

            var applicationName= root.GetSection(ApplicationiName).Get<string>();
            Xunit.Assert.Equal(applicationName, env);
            if (env.Equals("Development"))
            {
                var custom = root.GetSection("Custom").Get<string>();
                Xunit.Assert.Equal("MSSQL", custom);
            }
        }
    }
}
