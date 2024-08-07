﻿using SAE.Framework.Abstract.Mediator;
using System.Threading.Tasks;

namespace SAE.Framework.Mediator.Orleans.Product
{
    public class ProductHandler : ICommandHandler<ProductCommand>,
                                  ICommandHandler<ProductCommand, Product>
    {
        public Task HandleAsync(ProductCommand command)
        {
            return Task.CompletedTask;
        }

        

        Task<Product> ICommandHandler<ProductCommand, Product>.HandleAsync(ProductCommand command)
        {
            return Task.FromResult(new Product
            {
                OrderId=command.Id
            });
        }
    }
}
