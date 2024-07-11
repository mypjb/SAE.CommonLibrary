using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Abstract.Mediator
{
    /// <summary>
    /// 中介处理接口，此接口用于标记，请勿直接使用
    /// </summary>
    [Obsolete("中介处理接口，此接口用于标记，请使用ICommandHandler接口")]
    public interface IMediatorHandler
    {
    }
}
