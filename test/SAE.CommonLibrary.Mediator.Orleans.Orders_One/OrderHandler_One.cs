using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_One
{
    public class OrderHandler_One : ICommandHandler<OrderCommand_One>,
                                ICommandHandler<OrderCommand_One, string>,
                                ICommandHandler<Command.Delete<Order_One>,bool>
    {
        public Task Handle(OrderCommand_One command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Handle(Command.Delete<Order_One> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        Task<string> ICommandHandler<OrderCommand_One, string>.Handle(OrderCommand_One command)
        {
            return Task.FromResult(command.Id);
        }

        
    }
}
