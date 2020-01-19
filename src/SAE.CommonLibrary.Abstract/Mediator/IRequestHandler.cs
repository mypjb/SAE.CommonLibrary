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
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IRequestHandler<TCommand,TResponse> : IMediatorHandler where TCommand : class
    {
        /// <summary>
        /// 处理<paramref name="command"/>命令并返回<typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<TResponse> Handle(TCommand command);
    }
}
