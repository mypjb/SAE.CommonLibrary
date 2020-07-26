using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_Two
{
    public class OrderHandler_Two : ICommandHandler<OrderCommand_Two>,
                                ICommandHandler<OrderCommand_Two, string>,
                                ICommandHandler<Command.Delete<Order>,bool>
    {
        public Task Handle(OrderCommand_Two command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Handle(Command.Delete<Order> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        Task<string> ICommandHandler<OrderCommand_Two, string>.Handle(OrderCommand_Two command)
        {
            return Task.FromResult(command.Id);
        }

        
    }
}
