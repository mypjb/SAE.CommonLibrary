using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_Three
{
    public class OrderHandler_Three : ICommandHandler<OrderCommand_Three>,
                                ICommandHandler<OrderCommand_Three, string>,
                                ICommandHandler<Command.Delete<Order_Three>,bool>
    {
        public Task Handle(OrderCommand_Three command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Handle(Command.Delete<Order_Three> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        Task<string> ICommandHandler<OrderCommand_Three, string>.Handle(OrderCommand_Three command)
        {
            return Task.FromResult(command.Id);
        }

        
    }
}
