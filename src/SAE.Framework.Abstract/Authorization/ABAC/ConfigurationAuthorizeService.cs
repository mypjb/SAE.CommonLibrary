using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.Framework.Caching;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 默认授权服务
    /// </summary>
    /// <inheritdoc/>
    public abstract class ConfigurationAuthorizeService<TAuthDescriptor> : IAuthorizeService where TAuthDescriptor : AuthDescriptor
    {
        /// <summary>
        /// 授权策略集
        /// </summary>
        protected readonly IOptionsMonitor<List<AuthorizationPolicy>> _authorizationPoliciesOptionsMonitor;
        /// <summary>
        /// 授权描述集
        /// </summary>
        protected readonly IOptionsMonitor<List<TAuthDescriptor>> _authDescriptorsOptionsMonitor;
        /// <summary>
        /// 缓存
        /// </summary>
        protected readonly IMemoryCache _memoryCache;
        /// <summary>
        /// 规则上下文
        /// </summary>
        protected readonly IRuleContextFactory _ruleContextFactory;
        /// <summary>
        /// 规则上下文
        /// </summary>
        protected readonly IRuleDecoratorBuilder _ruleDecoratorBuilder;
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="authorizationPoliciesOptionsMonitor">授权策略集</param>
        /// <param name="authDescriptorsOptionsMonitor">授权描述集</param>
        /// <param name="memoryCache">缓存</param>
        /// <param name="ruleContextFactory">规则上下文</param>
        /// <param name="ruleDecoratorBuilder">规则构建对象</param>
        /// <param name="logging">日志记录器</param>
        public ConfigurationAuthorizeService(IOptionsMonitor<List<AuthorizationPolicy>> authorizationPoliciesOptionsMonitor,
                                             IOptionsMonitor<List<TAuthDescriptor>> authDescriptorsOptionsMonitor,
                                             IMemoryCache memoryCache,
                                             IRuleContextFactory ruleContextFactory,
                                             IRuleDecoratorBuilder ruleDecoratorBuilder,
                                             ILogging logging)
        {
            this._authorizationPoliciesOptionsMonitor = authorizationPoliciesOptionsMonitor;
            this._authDescriptorsOptionsMonitor = authDescriptorsOptionsMonitor;
            this._memoryCache = memoryCache;
            this._ruleContextFactory = ruleContextFactory;
            this._ruleDecoratorBuilder = ruleDecoratorBuilder;
            this._logging = logging;

            this._authDescriptorsOptionsMonitor.OnChange((_) => this.ClearCacheAsync().GetAwaiter().GetResult());

            this._authorizationPoliciesOptionsMonitor.OnChange((_) => this.ClearCacheAsync().GetAwaiter().GetResult());
        }

        /// <summary>
        /// 授权数据变更后，触发缓存清理动作。
        /// </summary>
        protected virtual async Task ClearCacheAsync()
        {
            await this._memoryCache.ClearAsync();
        }
        /// <inheritdoc/>
        public async Task<bool> AuthAsync()
        {
            bool result;
            var authDescriptor = await this.GetAuthDescriptorAsync();

            var context = await this._ruleContextFactory.GetAsync();

            if (authDescriptor == null)
            {
                this._logging.Info($"本次请求尚未匹配授权符，采用默认规则进行授权。");
                result = await this.DefaultAuthAsync(context);
            }
            else
            {
                this._logging.Info($"获得当前授权描述符:{authDescriptor.Key}");

                this._logging.Debug($"{nameof(authDescriptor)}:{authDescriptor.ToJsonString()}");

                var contextString = context.ToString();

                this._logging.Debug($"当前规则上下文:{contextString}");

                return await this.AuthCoreAsync(context, authDescriptor);
            }

            this._logging.Info($"授权{(result ? "成功" : "失败")}!");

            return result;
        }
        /// <summary>
        /// 授权核心函数
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="authDescriptor">描述符</param>
        /// <returns>true:认证成功，false:失败</returns>
        protected async Task<bool> AuthCoreAsync(RuleContext context, AuthDescriptor authDescriptor)
        {
            var authorizationPolicies = await this.GetAuthorizationPoliciesAsync();

            if (authorizationPolicies.Length > 0)
            {
                this._logging.Info($"匹配到授权策略:{authorizationPolicies.Length}");

                var policies = authorizationPolicies.Where(p => authDescriptor.PolicyIds.Contains(p.Id))
                                                                .ToArray();

                foreach (var policy in policies)
                {
                    var policyMessage = $"{policy.Id}-{policy.Name}";

                    this._logging.Debug($"准备使用策略({policyMessage})进行授权:{policy.Rule}");

                    var decorator = this._ruleDecoratorBuilder.Build(policy.Rule);

                    if (decorator != null)
                    {
                        await decorator.DecorateAsync(context);

                        if (context.Complete)
                        {
                            this._logging.Info($"策略授权成功:{policyMessage}");
                            return true;
                        }
                    }

                    this._logging.Info($"策略授权失败:{policyMessage}");
                }
            }
            else
            {
                this._logging.Info("不存在授权策略,准备使用默认授权规则！");
                return await this.DefaultAuthAsync(context);
            }

            return false;
        }
        /// <summary>
        /// 获得授权描述符
        /// </summary>
        /// <returns>授权描述符</returns>
        public virtual async Task<AuthDescriptor> GetAuthDescriptorAsync()
        {
            var key = await this.GetAuthDescriptorKeyAsync();

            this._logging.Info($"使用key查找授权描述符集合:{key}");

            return await this._memoryCache.GetOrAddAsync($"{Constants.Cache.GetAuthDescriptor}{key}", async () =>
            {
                var authDescriptors = this._authDescriptorsOptionsMonitor.CurrentValue ?? new List<TAuthDescriptor>();

                if (authDescriptors.Count > 0)
                {
                    this._logging.Warn($"找到授权描述符集合:{authDescriptors.Count}");

                    return await this.FindAuthDescriptorCoreAsync(key, authDescriptors);
                }
                else
                {
                    this._logging.Warn("未找到授权描述符集合");
                    return null;
                }
            }, CacheTime.OneYear);
        }
        /// <summary>
        /// 获得授权策略
        /// </summary>
        /// <returns>授权策略集合</returns>
        public virtual Task<AuthorizationPolicy[]> GetAuthorizationPoliciesAsync()
        {
            return Task.FromResult(this._authorizationPoliciesOptionsMonitor.CurrentValue?.ToArray() ?? new AuthorizationPolicy[0]);
        }
        /// <summary>
        /// 获得<see cref="AuthDescriptor"/>唯一标识:<see cref="AuthDescriptor.Key"/>
        /// </summary>
        /// <returns>当前授权描述符key</returns>
        protected abstract Task<string> GetAuthDescriptorKeyAsync();

        /// <summary>
        /// 默认授权，默认不通过
        /// </summary>
        /// <returns>true:认证成功，false:失败</returns>
        protected virtual Task<bool> DefaultAuthAsync(RuleContext context)
        {
            return Task.FromResult(false);
        }
        /// <summary>
        /// 获得<see cref="AuthDescriptor"/>核心函数
        /// </summary>
        /// <param name="key">
        /// 查找的key
        /// </param>
        /// <param name="authDescriptors">
        /// 描述符集合
        /// </param>
        /// <returns>授权描述符</returns>
        protected virtual Task<AuthDescriptor> FindAuthDescriptorCoreAsync(string key, IEnumerable<AuthDescriptor> authDescriptors)
        {
            var authDescriptor = authDescriptors.FirstOrDefault(s => s.Comparison(key));

            return Task.FromResult(authDescriptor);
        }
    }
}