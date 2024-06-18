using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.MessageQueue
{
    /// <summary>
    /// 标记接口，请勿之间引用
    /// </summary>
    public interface IHandler
    {

    }
    /// <summary>
    /// 处理接口
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IHandler<TMessage> : IHandler
    {
        /// <summary>
        /// 执行处理程序
        /// </summary>
        /// <param name="message">消息</param>
        Task HandleAsync(TMessage message);
    }


}
