using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Abstract.Responsibility
{
    /// <summary>
    /// 链条上下文
    /// </summary>
    public abstract class ResponsibilityContext
    {
        /// <summary>
        /// 链条处理完成
        /// </summary>
        /// <remarks>
        /// 只有调用<see cref="Success"/>函数它的值才会变成true
        /// </remarks>
        public bool Complete { get; protected set; }
        /// <summary>
        /// 执行成功
        /// </summary>
        public virtual void Success()
        {
            this.Complete = true;
        }
    }
}
