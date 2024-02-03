using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    public class DecoratorContext
    {
        /// <summary>
        /// 手动调用<see cref="Success"/>函数，返回true,否则false
        /// </summary>
        public bool Complete { get; protected set; }
        /// <summary>
        /// 成功
        /// </summary>
        public virtual void Success()
        {
            this.Complete = true;
        }
    }
}
