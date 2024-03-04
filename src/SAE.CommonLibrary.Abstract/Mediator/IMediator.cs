using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 中介者
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// 发送处理
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task SendAsync(object command,Type commandType);
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="responseType">响应类型</param>
        /// <returns>响应</returns>
        Task<object> SendAsync(object command,Type commandType,Type responseType);
    }
}
