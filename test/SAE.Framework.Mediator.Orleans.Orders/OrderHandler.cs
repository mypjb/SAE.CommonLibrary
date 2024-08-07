﻿using SAE.Framework.Abstract.Mediator;
using SAE.Framework.EventStore.Document;
using System;
using System.Threading.Tasks;

namespace SAE.Framework.Mediator.Orleans.Orders
{
    public class OrderHandler : ICommandHandler<OrderCommand>,
                                ICommandHandler<OrderCommand, Order>,
                                ICommandHandler<Command.Delete<Order>, bool>
    {
        //private readonly IMediator _mediator;

        //public OrderHandler(IMediator mediator)
        //{
        //    this._mediator = mediator;
        //}
        public Task HandleAsync(OrderCommand command)
        {
            return Task.CompletedTask;
        }

        public Task<bool> HandleAsync(Command.Delete<Order> command)
        {
            Console.WriteLine(command.Id);
            return Task.FromResult(true);
        }

        async Task<Order> ICommandHandler<OrderCommand, Order>.HandleAsync(OrderCommand command)
        {
            //var product = await this._mediator.Send<Product.Product>(new ProductCommand());
            return new Order
            {
                Id = command.Id,
                //Products = new[] { product }
            };
        }


    }
}
