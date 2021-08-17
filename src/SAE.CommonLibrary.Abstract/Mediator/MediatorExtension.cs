using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{

    /// <summary>
    /// 
    /// </summary>
    public static class MediatorExtension
    {
        public static Task SendAsync(this IMediator mediator,object command)
        {
            return mediator.SendAsync(command, command.GetType());
        }
        public static Task SendAsync<TCommand>(this IMediator mediator,TCommand command)
        {
            var commandType = typeof(TCommand);
            return mediator.SendAsync(command, commandType);
        }
        public static async Task<TResponse> SendAsync<TCommand,TResponse>(this IMediator mediator, TCommand command)
        {
            var commandType = typeof(TCommand);
            return (TResponse)(await mediator.SendAsync(command, commandType, typeof(TResponse)));
        }
        public static async Task<TResponse> SendAsync<TResponse>(this IMediator mediator, object command)
        {
            var commandType = command.GetType();
            return (TResponse)(await mediator.SendAsync(command, commandType, typeof(TResponse)));
        }
    }
}
