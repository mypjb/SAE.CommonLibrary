using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.MessageQueue
{
    /// <summary>
    /// 消息标识
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// 消息标识
        /// </summary>
        public string Identity { get;  }
    }
}
