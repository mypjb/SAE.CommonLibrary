using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.AspNetCore.Authorization.ABAC;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class ABACAuthorizeTest : BaseTest
    {
        private readonly IRuleDecoratorBuilder _ruleDecoratorBuilder;

        public ABACAuthorizeTest(ITestOutputHelper output) : base(output)
        {
            this._ruleDecoratorBuilder = this._serviceProvider.GetService<IRuleDecoratorBuilder>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRuleDecoratorBuilder, DefaultRuleDecoratorBuilder>();

            base.ConfigureServices(services);
        }

        [Theory]
        [InlineData("18 >= $age || 'student' != $role || 'pjb' == $name")]
        public async Task AuthAsync(string arg)
        {
            var decorator = this._ruleDecoratorBuilder.Build(arg);
        }
    }
}