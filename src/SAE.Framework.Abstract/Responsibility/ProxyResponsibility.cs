using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Responsibility
{
    /// <summary>
    /// 职责链代理实现
    /// </summary>
    /// <typeparam name="TContext">上下文</typeparam>
    public class ProxyResponsibility<TContext> : AbstractResponsibility<TContext> where TContext : ResponsibilityContext
    {
        /// <summary>
        /// 代理
        /// </summary>
        private readonly IResponsibility<TContext> _proxy;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="responsibility">链条</param>
        public ProxyResponsibility(IResponsibility<TContext> responsibility)
        {
            Assert.Build(responsibility, nameof(responsibility))
                  .NotNull();
            this._proxy = responsibility;
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="responsibilities">链条集合</param>
        public ProxyResponsibility(IEnumerable<IResponsibility<TContext>> responsibilities)
        {
            Assert.Build(responsibilities, nameof(responsibilities))
                  .NotNull()
                  .Then(s => s.Any(), nameof(responsibilities))
                  .True();

            this._proxy = responsibilities.First();

            for (var i = 1; i < responsibilities.Count(); i++)
            {
                this.Add(responsibilities.ElementAt(i));
            }
        }

        ///<inheritdoc/>
        public override async Task HandleAsync(TContext context)
        {
            await this._proxy.HandleAsync(context);
            await base.HandleAsync(context);
        }

    }
}
