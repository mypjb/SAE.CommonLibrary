using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public class AbstractResponsibility<TContext> : IResponsibility<TContext> where TContext : ResponsibilityContext
    {
        protected IResponsibility<TContext> Responsibility { get; private set; }
        public AbstractResponsibility()
        {

        }

        /// <summary>
        /// 添加一个新的<paramref name="responsibility"/>链条进来
        /// </summary>
        /// <param name="responsibility"></param>
        public virtual void Add(IResponsibility<TContext> responsibility)
        {
            if (responsibility == null) return;

            var proxy = responsibility as ProxyResponsibility<TContext>;

            if (proxy == null)
            {
                proxy = new ProxyResponsibility<TContext>(responsibility);
            }

            if (this.Responsibility == null)
            {
                this.Responsibility = proxy;
            }
            else
            {
                ((ProxyResponsibility<TContext>)this.Responsibility).Add(proxy);
            }
        }
        /// <summary>
        /// 送上至下执行链条知道知道结果为止
        /// </summary>
        /// <param name="context"></param>
        public virtual async Task HandleAsync(TContext context)
        {
            if (context.Complete || this.Responsibility == null) return;
            await this.Responsibility.HandleAsync(context);
        }
    }
}
