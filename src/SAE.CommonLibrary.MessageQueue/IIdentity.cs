using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.MessageQueue
{
    /// <summary>
    /// 消息标识
    /// </summary>
    public interface IIdentity
    {
        public string Identity { get;  }
    }
}
