using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    public class DecoratorContext
    {
        /// <summary>
        /// 提前完成该
        /// </summary>
        public bool Complete { get; protected set; }
        /// <summary>
        /// 提前完成
        /// </summary>
        public virtual void Success()
        {
            this.Complete = true;
        }
    }
}
