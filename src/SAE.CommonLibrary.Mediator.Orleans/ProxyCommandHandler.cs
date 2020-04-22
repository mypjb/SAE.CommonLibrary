using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
namespace SAE.CommonLibrary.Mediator.Orleans
{
    internal class ProxyCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : class
    {
        private readonly IClusterClient _client;

        public ProxyCommandHandler(IClusterClient client)
        {
            this._client = client;
        }
        public Task Handle(TCommand command)
        {
            var grain = this._client.GetGrain<IGrainCommandHandler>(command.GetType().GUID.ToString());
            return grain.Send(command);
        }
    }

    internal class ProxyCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand : class
    {
        private readonly IClusterClient _client;

        public ProxyCommandHandler(IClusterClient client)
        {
            this._client = client;
        }

        public async Task<TResponse> Handle(TCommand command)
        {
            var grain = this._client.GetGrain<IGrainCommandHandler>(command.GetType().GUID.ToString());

            var responseType = typeof(TResponse);

            var @object = await grain.Send(command, responseType);

            return (TResponse)@object;
        }
    }
}
