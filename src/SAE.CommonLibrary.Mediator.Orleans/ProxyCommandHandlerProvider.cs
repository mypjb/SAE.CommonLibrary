using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class ProxyCommandHandlerProvider : IProxyCommandHandlerProvider
    {
        private readonly ConcurrentDictionary<string, Task<IClusterClient>> _dictionary;
        private readonly OrleansOptions _options;
        private readonly IHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public ProxyCommandHandlerProvider(OrleansOptions options,
                                           IHostEnvironment environment,
                                           IServiceProvider serviceProvider,
                                           IMediator mediator)
        {
            this._mediator = mediator;
            this._dictionary = new ConcurrentDictionary<string, Task<IClusterClient>>();
            this._options = options;
            this._environment = environment;
            this._serviceProvider = serviceProvider;
        }

        private async Task<IClusterClient> ConfigurationClusterClient(string serviceId)
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
                .ConfigureApplicationParts(part =>
                {
                    part.AddApplicationPart(Assembly.GetExecutingAssembly());
                })
                .ConfigureServices(service =>
                {
                    service.AddSingleton(this._mediator);
                })
                .Build();

            await clusterClient.Connect(ex =>
            {
                return Task.FromResult(true);
            });

            return clusterClient;
        }


        public async Task<ICommandHandler<TCommand>> Get<TCommand>() where TCommand : class
        {
            var provider = this.GetProvider<TCommand>();
            var key = provider.Get();
            var clusterClient = _dictionary.GetOrAdd(key, this.ConfigurationClusterClient);
            return new ProxyCommandHandler<TCommand>(await clusterClient);
        }

        public async Task<ICommandHandler<TCommand, TResponse>> Get<TCommand, TResponse>() where TCommand : class
        {
            var provider = this.GetProvider<TCommand>();
            var key = provider.Get();
            var clusterClient = _dictionary.GetOrAdd(key, this.ConfigurationClusterClient);
            return new ProxyCommandHandler<TCommand, TResponse>(await clusterClient);
        }

        private IOrleansKeyProvider<TCommand> GetProvider<TCommand>()
        {
            var provider = this._serviceProvider.GetService<IOrleansKeyProvider<TCommand>>();
            return provider ?? new DefaultOrleansKeyProvider<TCommand>();
        }
    }
}
