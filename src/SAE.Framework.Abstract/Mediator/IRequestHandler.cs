using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 通过<seealso cref="IMediator.Send{TResponse}(object)"/>
    /// 调用命令并返回<typeparamref name="TResponse"/>
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    /// <typeparam name="TResponse">响应</typeparam>
    public interface IRequestHandler<TCommand,TResponse> : IMediatorHandler where TCommand : class
    {
        /// <summary>
        /// 处理<paramref name="command"/>命令并返回<typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>响应</returns>
        Task<TResponse> Handle(TCommand command);
    }
}
