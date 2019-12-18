using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public class ProxyResponsibility<TContext> : AbstractResponsibility<TContext> where TContext : ResponsibilityContext
    {
        private readonly IResponsibility<TContext> _proxy;

        public ProxyResponsibility(IResponsibility<TContext> responsibility)
        {
            Assert.Build(responsibility, nameof(responsibility))
                  .NotNull();
            this._proxy = responsibility;
        }

        public ProxyResponsibility(IEnumerable<IResponsibility<TContext>> responsibilitys)
        {
            Assert.Build(responsibilitys, nameof(responsibilitys))
                  .NotNull()
                  .Then(s => s.Any(), nameof(responsibilitys))
                  .True();

            this._proxy = responsibilitys.First();

            for (var i = 1; i < responsibilitys.Count(); i++)
            {
                this.Add(responsibilitys.ElementAt(i));
            }
        }

        /// <summary>
        /// 送上至下执行链条知道知道结果为止
        /// </summary>
        /// <param name="context"></param>
        public override async Task HandleAsync(TContext context)
        {
            await this._proxy.HandleAsync(context);
            await base.HandleAsync(context);
        }

    }
}
