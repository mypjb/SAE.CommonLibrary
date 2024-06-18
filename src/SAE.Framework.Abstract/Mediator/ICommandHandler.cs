using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Mediator
{
    /// <summary>
    /// 通过<seealso cref="IMediator.SendAsync(object, Type)"/>调用命令
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    public interface ICommandHandler<TCommand> : IMediatorHandler
    {
        /// <summary>
        /// 处理<typeparamref name="TCommand"/>命令
        /// </summary>
        /// <param name="command">命令类型</param>
        Task HandleAsync(TCommand command);
    }

    /// <summary>
    /// 通过<seealso cref="IMediator.SendAsync(object, Type, Type)"/>
    /// 调用命令并返回<typeparamref name="TResponse"/>
    /// </summary>
    /// <typeparam name="TCommand">命令类型</typeparam>
    /// <typeparam name="TResponse">响应类型</typeparam>
    public interface ICommandHandler<TCommand, TResponse> : IMediatorHandler where TCommand : class
    {
        /// <summary>
        /// 处理<paramref name="command"/>命令并返回<typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>响应</returns>
        Task<TResponse> HandleAsync(TCommand command);
    }
}
