using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public class ProxyResponsibility<TContext>:AbstractResponsibility<TContext> where TContext :ResponsibilityContext
    {
        private readonly IResponsibility<TContext> _proxy;

        public ProxyResponsibility(IResponsibility<TContext> responsibility)
        {
            this._proxy = responsibility;
        }
        /// <summary>
        /// 送上至下执行链条知道知道结果为止
        /// </summary>
        /// <param name="context"></param>
        public override async Task HandleAsync(TContext context)
        {
            await this._proxy.HandleAsync(context);
            await this._proxy.HandleAsync(context);
        }

    }
}
