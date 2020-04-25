using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SAE.CommonLibrary.Abstract.Mediator;
using System.Collections.Concurrent;
using System.Reflection;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class ProxyCommandHandlerProvider : IProxyCommandHandlerProvider
    {
        private readonly ConcurrentDictionary<string, IClusterClient> _dictionary;
        private readonly OrleansOptions _options;
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment _environment;

        public ProxyCommandHandlerProvider(OrleansOptions options, Microsoft.Extensions.Hosting.IHostEnvironment environment)
        {
            this._dictionary = new ConcurrentDictionary<string, IClusterClient>();
            this._options = options;
            this._environment = environment;
        }

        private IClusterClient ConfigurationClusterClient(string serviceId)
        {

            var clientBuilder = new ClientBuilder();

            if (this._environment.EnvironmentName == Environments.Development)
            {
                clientBuilder.UseLocalhostClustering();
            }
            else
            {
                clientBuilder.UseZooKeeperClustering(zooKeeperOptions =>
                {
                    zooKeeperOptions.ConnectionString = this._options.ZooKeeperConnectionString;
                });
            }

            var clusterClient = clientBuilder.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = this._options.ClusterId;
                    options.ServiceId = serviceId;
                })
                .ConfigureApplicationParts(part => part.AddApplicationPart(Assembly.GetExecutingAssembly()))
                .Build();

            clusterClient.Connect().GetAwaiter().GetResult();

            return clusterClient;
        }


        public ICommandHandler<TCommand> Get<TCommand>() where TCommand : class
        {
            var key = Utility.GetIdentity(typeof(TCommand));
            var clusterClient = _dictionary.GetOrAdd(key, this.ConfigurationClusterClient);
            return new ProxyCommandHandler<TCommand>(clusterClient);
        }

        public ICommandHandler<TCommand, TResponse> Get<TCommand, TResponse>() where TCommand : class
        {
            var key = Utility.GetIdentity(typeof(TCommand));
            var clusterClient = _dictionary.GetOrAdd(key, this.ConfigurationClusterClient);
            return new ProxyCommandHandler<TCommand, TResponse>(clusterClient);
        }
    }
}
