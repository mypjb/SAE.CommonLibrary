using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IRuleContextFactory"/>默认实现
    /// </summary>
    /// <remarks>
    /// 该实现将会在内部通过<see cref="IRuleContextProvider"/>获取所有的<see cref="RuleContext"/>,
    /// 之后会返回合并后的<see cref="RuleContext"/>。
    /// </remarks>
    public class DefaultRuleContextFactory : IRuleContextFactory
    {
        /// <summary>
        /// 规则上下文提供者
        /// </summary>
        private readonly IEnumerable<IRuleContextProvider> _providers;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="providers">规则上下文集合</param>
        public DefaultRuleContextFactory(IEnumerable<IRuleContextProvider> providers)
        {
            this._providers = providers;

        }
        ///<inheritdoc/>
        public virtual async Task<RuleContext> GetAsync()
        {
            var ctx = new RuleContext();
            foreach (var provider in this._providers)
            {
                ctx.Merge(await provider.GetAsync());
            }

            return ctx;
        }
    }
}