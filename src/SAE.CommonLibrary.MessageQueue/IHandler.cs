using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.MessageQueue
{
    /// <summary>
    /// 处理接口
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IHandler<TMessage>
    {
        /// <summary>
        /// 执行处理程序
        /// </summary>
        /// <param name="message"></param>
        Task HandleAsync(TMessage message);
    }

    
}
