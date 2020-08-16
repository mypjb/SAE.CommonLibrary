using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public class ResponsibilityProvider<TResponsibilityContext> : IResponsibilityProvider<TResponsibilityContext> 
        where TResponsibilityContext : ResponsibilityContext
    {
        public ResponsibilityProvider(IEnumerable<IResponsibility<TResponsibilityContext>> responsibilities)
        {
            this.Responsibilities = responsibilities;
            this.Root = this.Compose();
        }

        public IResponsibility<TResponsibilityContext> Root
        {
            get;
        }

        public IEnumerable<IResponsibility<TResponsibilityContext>> Responsibilities
        {
            get;
        }

        protected virtual IResponsibility<TResponsibilityContext> Compose()
        {
            if (!this.Responsibilities.Any())
            {
                throw new SaeException($" not register '{typeof(TResponsibilityContext).Name}' class IResponsibility");
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
