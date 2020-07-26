using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.Mediator.Orleans.Orders;
using SAE.CommonLibrary.Mediator.Orleans.Product;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class OrderHandler : ICommandHandler<OrderCommand>,
                                ICommandHandler<OrderCommand, Order>,
                                ICommandHandler<Command.Delete<Order>, bool>
    {
        private readonly IMediator _mediator;

        public OrderHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public Task Handle(OrderCommand command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Handle(Command.Delete<Order> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        async Task<Order> ICommandHandler<OrderCommand, Order>.Handle(OrderCommand command)
        {
            var product = await this._mediator.Send<Product.Product>(new ProductCommand());
            return new Order
            {
                Id = command.Id,
                Products = new[] { product }
            };
        }


    }
}
