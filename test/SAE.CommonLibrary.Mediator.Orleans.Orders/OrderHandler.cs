using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.Mediator.Orleans.Orders;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class OrderHandler : ICommandHandler<OrderCommand>,
                                ICommandHandler<OrderCommand, string>,
                                ICommandHandler<Command.Delete<Order>,bool>
    {
        public Task Handle(OrderCommand command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Handle(Command.Delete<Order> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        Task<string> ICommandHandler<OrderCommand, string>.Handle(OrderCommand command)
        {
            return Task.FromResult(command.Id);
        }

        
    }
}
