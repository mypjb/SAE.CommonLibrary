using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    internal class ProxyCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : class
    {
        private readonly IClusterClient _client;
        private readonly ILogging _logging;

        public ProxyCommandHandler(IClusterClient client, ILogging logging)
        {
            this._client = client;
            this._logging = logging;
        }
        public Task HandleAsync(TCommand command)
        {
            this._logging.Debug($"get grain {this._client.IsInitialized}");
            var grain = this._client.GetGrain<IGrainCommandHandler>("0");
            //this._logging.Info($"grain:{grain.GetGrainIdentity().ToJsonString()}");
            return grain.Send(command);
        }
    }

    internal class ProxyCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand : class
    {
        private readonly IClusterClient _client;
        private readonly ILogging _logging;

        public ProxyCommandHandler(IClusterClient client, ILogging logging)
        {
            this._client = client;
            this._logging = logging;
        }

        public async Task<TResponse> HandleAsync(TCommand command)
        {
            this._logging.Debug($"get grain {this._client.IsInitialized}");
            var grain = this._client.GetGrain<IGrainCommandHandler>("0");
            //this._logging.Info($"grain:{grain.GetGrainIdentity().}");
            var response = await grain.Send<TCommand, TResponse>(command);
            this._logging.Debug($"response:{response.ToJsonString()}");
            return response;
        }
    }
}
