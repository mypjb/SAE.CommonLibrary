using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// ABAC授权认证的实现
    /// </summary>
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly ILogging _logging;
        private readonly IRuleContextFactory _ruleContextFactory;
        private readonly IRuleDecoratorBuilder _ruleDecoratorBuilder;
        private readonly IAuthDescriptorProvider _authDescriptorProvider;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="ruleContextFactory"></param>
        /// <param name="ruleDecoratorBuilder"></param>
        /// <param name="authDescriptorProvider"></param>
        public AuthorizationHandler(ILogging<AuthorizationHandler> logging,
                                    IRuleContextFactory ruleContextFactory,
                                    IRuleDecoratorBuilder ruleDecoratorBuilder,
                                    IAuthDescriptorProvider authDescriptorProvider)
        {
            this._logging = logging;
            this._ruleContextFactory = ruleContextFactory;
            this._ruleDecoratorBuilder = ruleDecoratorBuilder;
            this._authDescriptorProvider = authDescriptorProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            var authDescriptor = await this._authDescriptorProvider.GetAsync();

            if (!context.User.Identity.IsAuthenticated)
            {
                this._logging.Info($"用户尚未认证!");
                return;
            }

            if (authDescriptor == null)
            {
                this._logging.Info("本次请求尚未匹配授权符，采用默认规则进行授权。");
            }
            else
            {
                var authDescriptorJson = $"{authDescriptor.ToJsonString()}";
                this._logging.Debug($"已找到授权规则：{authDescriptorJson}");

                var ruleContext = await this._ruleContextFactory.GetAsync();

                var ruleContextString = ruleContext.ToString();

                this._logging.Debug($"授权上下文：{ruleContextString}");

                var ruleDecorator = this._ruleDecoratorBuilder.Build(authDescriptor.Rule);

                await ruleDecorator.DecorateAsync(ruleContext);

                this._logging.Info($"授权信息：{nameof(ruleContext)}:{ruleContextString},{nameof(authDescriptor)}:{authDescriptorJson}");

                var path = $"{authDescriptor.Method}:{authDescriptor.Path}";

                if (!ruleContext.Complete)
                {
                    this._logging.Info($"授权失败:{path}");
                    return;
                }

                this._logging.Info($"授权成功:{path}");
            }

            context.Succeed(requirement);
        }
    }
}