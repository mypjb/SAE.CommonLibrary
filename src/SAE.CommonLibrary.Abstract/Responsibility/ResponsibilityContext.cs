using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    /// <summary>
    /// Responsibility Context 
    /// </summary>
    public abstract class ResponsibilityContext
    {
        /// <summary>
        /// handler whether complete
        /// </summary>
        public bool Complete { get; protected set; }
        /// <summary>
        /// handler success
        /// </summary>
        public virtual void Success()
        {
            this.Complete = true;
        }
    }
}
