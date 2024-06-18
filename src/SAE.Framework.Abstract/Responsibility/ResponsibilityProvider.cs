using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.Framework.Abstract.Responsibility
{
    /// <summary>
    /// 职责链提供者
    /// </summary>
    /// <typeparam name="TResponsibilityContext">上下文</typeparam>
    public class ResponsibilityProvider<TResponsibilityContext> : IResponsibilityProvider<TResponsibilityContext>
        where TResponsibilityContext : ResponsibilityContext
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="responsibilities">链条集合</param>
        public ResponsibilityProvider(IEnumerable<IResponsibility<TResponsibilityContext>> responsibilities)
        {
            this.Responsibilities = responsibilities.OrderByDescending(s => s, OrderComparer.Comparer)
                                                    .ToArray();
            this.Root = this.Compose();
        }
        /// <inheritdoc/>
        public IResponsibility<TResponsibilityContext> Root
        {
            get;
        }
        /// <inheritdoc/>
        public IEnumerable<IResponsibility<TResponsibilityContext>> Responsibilities
        {
            get;
        }
        /// <summary>
        /// 将链条进行组合
        /// </summary>
        /// <returns>返回组合好的链条</returns>
        /// <exception cref="SAEException">如果一个链条都未注册,则会触发异常</exception>
        protected virtual IResponsibility<TResponsibilityContext> Compose()
        {
            if (!this.Responsibilities.Any())
            {
                throw new SAEException($" not register '{typeof(TResponsibilityContext).Name}' class IResponsibility");
            }

            var first = this.Responsibilities.First();
            AbstractResponsibility<TResponsibilityContext> responsibility;
            if (first is AbstractResponsibility<TResponsibilityContext>)
            {
                responsibility = (AbstractResponsibility<TResponsibilityContext>)first;
            }
            else
            {
                responsibility = new ProxyResponsibility<TResponsibilityContext>(first);
            }

            if (this.Responsibilities.Count() > 1)
            {
                for (int i = 1; i < this.Responsibilities.Count(); i++)
                {
                    responsibility.Add(this.Responsibilities.ElementAt(i));
                }
            }

            return responsibility;
        }
    }
}
