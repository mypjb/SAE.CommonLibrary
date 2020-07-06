using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public static class OrleansMediatorExtension
    {
        public static Task Send(this IGrainCommandHandler commandHandler, object command)
        {
            return commandHandler.Send(command, command.GetType());
        }
        public static Task Send<TCommand>(this IGrainCommandHandler commandHandler, TCommand command)
        {
            var commandType = typeof(TCommand);
            return commandHandler.Send(command, commandType);
        }
        public static async Task<TResponse> Send<TCommand, TResponse>(this IGrainCommandHandler commandHandler, TCommand command)
        {
            var commandType = typeof(TCommand);
            return (TResponse)(await commandHandler.Send(command, commandType, typeof(TResponse)));
        }
        public static async Task<TResponse> Send<TResponse>(this IGrainCommandHandler commandHandler, object command)
        {
            var commandType = command.GetType();
            return (TResponse)(await commandHandler.Send(command, commandType, typeof(TResponse)));
        }
    }
}
