using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public interface IProxyCommandHandlerProvider
    {
        ICommandHandler<TCommand> Get<TCommand>() where TCommand : class;

        ICommandHandler<TCommand, TResponse> Get<TCommand, TResponse>() where TCommand : class;
    }
}
