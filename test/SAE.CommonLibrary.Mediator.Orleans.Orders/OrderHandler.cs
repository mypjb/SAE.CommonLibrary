using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Mediator.Orleans.Orders;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class OrderHandler : ICommandHandler<OrderCommand>,
                                ICommandHandler<OrderCommand, string>
    {
        public Task Handle(OrderCommand command)
        {
            return Task.CompletedTask;
        }

        Task<string> ICommandHandler<OrderCommand, string>.Handle(OrderCommand command)
        {
            return Task.FromResult(command.Id);
        }
    }
}
