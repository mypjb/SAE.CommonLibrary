using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public interface IProxyCommandHandlerProvider
    {
        Task<ICommandHandler<TCommand>> Get<TCommand>() where TCommand : class;

        Task<ICommandHandler<TCommand, TResponse>> Get<TCommand, TResponse>() where TCommand : class;
    }
}
