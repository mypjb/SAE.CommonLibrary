using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    public abstract class ResponsibilityContext
    {
        public bool Complete { get; protected set; }

        public virtual void Success()
        {
            this.Complete = true;
        }
    }
}
