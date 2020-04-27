using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans.Product
{
    public class ProductHandler : ICommandHandler<ProductCommand>,
                                  ICommandHandler<ProductCommand,string>
    {
        public Task Handle(ProductCommand command)
        {
            return Task.CompletedTask;
        }

        Task<string> ICommandHandler<ProductCommand, string>.Handle(ProductCommand command)
        {
            return Task.FromResult(command.Id);
        }
    }
}
