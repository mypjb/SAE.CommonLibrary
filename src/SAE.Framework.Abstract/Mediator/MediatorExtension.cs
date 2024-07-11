using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Mediator
{

    /// <summary>
    /// 中介者扩展
    /// </summary>
    public static class MediatorExtension
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="mediator">中介</param>
        /// <param name="command">命令</param>
        /// <returns></returns>
        public static Task SendAsync(this IMediator mediator,object command)
        {
            return mediator.SendAsync(command, command.GetType());
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="mediator">中介</param>
        /// <param name="command">命令</param>
        public static Task SendAsync<TCommand>(this IMediator mediator,TCommand command)
        {
            var commandType = typeof(TCommand);
            return mediator.SendAsync(command, commandType);
        }
        
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="mediator">中介</param>
        /// <param name="command">命令</param>
        /// <returns>响应</returns>
        public static async Task<TResponse> SendAsync<TCommand,TResponse>(this IMediator mediator, TCommand command)
        {
            var commandType = typeof(TCommand);
            return (TResponse)(await mediator.SendAsync(command, commandType, typeof(TResponse)));
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="mediator">中介</param>
        /// <param name="command">命令</param>
        /// <returns>响应</returns>
        public static async Task<TResponse> SendAsync<TResponse>(this IMediator mediator, object command)
        {
            var commandType = command.GetType();
            return (TResponse)await mediator.SendAsync(command, commandType, typeof(TResponse));
        }
    }
}
