﻿using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Proxy;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    internal abstract class CommandHandlerWrapper
    {
        public abstract Task Invoke(object command);
    }

    internal class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper where TCommand : class
    {
        private readonly IEnumerable<ICommandHandler<TCommand>> _handlers;

        public CommandHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._handlers = serviceProvider.GetServices<ICommandHandler<TCommand>>();
        }

        public override async Task Invoke(object command)
        {
            TCommand arg = (TCommand)command;
            await _handlers.ForEachAsync(async handler => await handler.Handle(arg));
        }
    }
}
