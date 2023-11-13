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
        [InlineData("18 <= $age || 'student' != $role || 'pjb' == $name",
                    "{age:'19',role:'student',name:'mypjb'}")]
        [InlineData("18 <= $age || 'student' != $role || 'pjb' == $name",
                    "{age:'17',role:'admin',name:'mypjb'}")]
        [InlineData("18 <= $age || 'student' != $role || 'pjb' == $name",
                    "{age:'16',role:'student',name:'pjb'}")]
        [InlineData("18 <= $age && 'student' != $role || 'pjb' == $name",
                    "{age:'99',role:'teacher',name:'mypjb'}")]
        [InlineData("18 <= $age || 'student' != $role && 'pjb' == $name",
                    "{age:'19',role:'student',name:'mypjb'}")]
        [InlineData("18 <= $age || 'student' != $role && 'pjb' == $name",
                    "{age:'10',role:'teacher',name:'pjb'}")]
        public async Task AuthOkAsync(string arg, string ctxJson)
        {
            var ctxDict = ctxJson.ToObject<Dictionary<string, string>>();

            var decorator = this._ruleDecoratorBuilder.Build(arg);

            var ctx = new RuleContext(ctxDict);

            await decorator.DecorateAsync(ctx);

            Xunit.Assert.True(ctx.Complete);
        }

        [Theory]
        [InlineData("18 <= $age || 'student' != $role || 'pjb' == $name",
                    "{age:'17',role:'student',name:'mypjb'}")]
        [InlineData("18 <= $age && 'student' != $role || 'pjb' == $name",
                    "{age:'99',role:'student',name:'pjb'}")]
        [InlineData("18 <= $age || 'student' != $role && 'pjb' == $name",
                    "{age:'17',role:'student',name:'mypjb'}")]
        [InlineData("18 <= $age || 'student' != $role && 'pjb' == $name",
                    "{age:'10',role:'student',name:'pjb'}")]
        public async Task AuthFailAsync(string arg, string ctxJson)
        {
            var ctxDict = ctxJson.ToObject<Dictionary<string, string>>();

            var decorator = this._ruleDecoratorBuilder.Build(arg);

            var ctx = new RuleContext(ctxDict);

            await decorator.DecorateAsync(ctx);

            Xunit.Assert.False(ctx.Complete);
        }
    }
}