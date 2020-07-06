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
        public static Task Send(this IMediator mediator,object command)
        {
            return mediator.Send(command, command.GetType());
        }
        public static Task Send<TCommand>(this IMediator mediator,TCommand command)
        {
            var commandType = typeof(TCommand);
            return mediator.Send(command, commandType);
        }
        public static async Task<TResponse> Send<TCommand,TResponse>(this IMediator mediator, TCommand command)
        {
            var commandType = typeof(TCommand);
            return (TResponse)(await mediator.Send(command, commandType, typeof(TResponse)));
        }
        public static async Task<TResponse> Send<TResponse>(this IMediator mediator, object command)
        {
            var commandType = command.GetType();
            return (TResponse)(await mediator.Send(command, commandType, typeof(TResponse)));
        }
    }
}
