using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 通过<seealso cref="IMediator.Send{TResponse}(object)"/>调用命令
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<TCommand>:IMediatorHandler
    {
        /// <summary>
        /// 处理<typeparamref name="TCommand"/>命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task Handle(TCommand command);
    }
}
