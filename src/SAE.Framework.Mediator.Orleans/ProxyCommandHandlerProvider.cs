//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Orleans;
//using Orleans.Configuration;
//using Orleans.Hosting;
//using SAE.Framework.Abstract.Mediator;
//using System;
//using System.Collections.Concurrent;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Threading.Tasks;
//using org.apache.zookeeper;
//using Microsoft.Extensions.Logging;
//using SAE.Framework.Logging;
//using SAE.Framework.Extension;
//using Microsoft.Extensions.Options;

//namespace SAE.Framework.Mediator.Orleans
//{
//    public class ProxyCommandHandlerProvider : IProxyCommandHandlerProvider
//    {
//        private readonly ConcurrentDictionary<string, IClusterClient> _dictionary;
//        private readonly OrleansOptions _options;
//        private readonly IHostEnvironment _environment;
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IMediator _mediator;

//        private readonly ILoggingFactory _loggingFactory;

//        private readonly ILogging _logging;

//        public ProxyCommandHandlerProvider(IOptions<OrleansOptions> options,
//                                           ILogging<ProxyCommandHandlerProvider> logging,
//                                           IHostEnvironment environment,
//                                           IServiceProvider serviceProvider,
//                                           ILoggingFactory loggingFactory,
//                                           IMediator mediator)
//        {
//            this._mediator = mediator;
//            this._dictionary = new ConcurrentDictionary<string, IClusterClient>();
//            this._options = options.Value;
//            this._logging = logging;
//            this._environment = environment;
//            this._serviceProvider = serviceProvider;
//            this._loggingFactory = loggingFactory;
//        }

//        private async Task<IClusterClient> ConfigurationClusterClient(string serviceId)
//        {

//            var clientBuilder = new ClientBuilder();

//            if (this._environment.EnvironmentName == Environments.Development)
//            {
//                clientBuilder.UseLocalhostClustering(serviceId: this._options.ClusterId, clusterId: serviceId.ToLower());
//            }
//            else
//            {
//                clientBuilder.UseZooKeeperClustering(zooKeeperOptions =>
//                {
//                    zooKeeperOptions.ConnectionString = this._options.ZooKeeperConnectionString;
//                });
//            }

//            var clusterClient = clientBuilder
//                //.Configure<ClusterOptions>(options =>
//                //{
//                //    options.ClusterId = this._options.ClusterId;
//                //    options.ServiceId = serviceId.ToLower();
//                //})
//                .ConfigureApplicationParts(part =>
//                {
//                    part.AddApplicationPart(Assembly.GetExecutingAssembly());
//                })
//                .ConfigureServices(service =>
//                {
//                    service.AddLogger(this._loggingFactory)
//                           .AddMicrosoftLogging()
//                           .AddSingleton(this._mediator);
//                })
//                .ConfigureLogging(configure => configure.SetMinimumLevel(this._environment.EnvironmentName == Environments.Development ? LogLevel.Debug : LogLevel.Information))
//                .Build();

//            this._logging.Info($"begin connect Orleans client");
//            await clusterClient.Connect(ex =>
//            {
//                return Task.FromResult(true);
//            });
//            this._logging.Info($"OrleansClient:{clusterClient.ToJsonString()}");
//            return clusterClient;
//        }


//        public async Task<ICommandHandler<TCommand>> Get<TCommand>() where TCommand : class
//        {
//            var provider = this.GetProvider<TCommand>();
//            var key = provider.Get();
//            IClusterClient clusterClient;
//            if(!_dictionary.TryGetValue(key,out clusterClient))
//            {
//                clusterClient = await this.ConfigurationClusterClient(key);
//                this._dictionary.AddOrUpdate(key, clusterClient, (a, b) => clusterClient);
//            }
//            return new ProxyCommandHandler<TCommand>(
//                                clusterClient, 
//                                this._loggingFactory.Create<ProxyCommandHandler<TCommand>>());
//        }

//        public async Task<ICommandHandler<TCommand, TResponse>> Get<TCommand, TResponse>() where TCommand : class
//        {
//            var provider = this.GetProvider<TCommand>();
//            var key = provider.Get();
//            IClusterClient clusterClient;
//            if (!_dictionary.TryGetValue(key, out clusterClient))
//            {
//                clusterClient = await this.ConfigurationClusterClient(key);
//                this._dictionary.AddOrUpdate(key, clusterClient, (a, b) => clusterClient);
//            }
//            return new ProxyCommandHandler<TCommand, TResponse>(
//                                            clusterClient,
//                                            this._loggingFactory.Create<ProxyCommandHandler<TCommand>>());
//        }

//        private IOrleansKeyProvider<TCommand> GetProvider<TCommand>()
//        {
//            var provider = this._serviceProvider.GetService<IOrleansKeyProvider<TCommand>>();
//            return provider ?? new DefaultOrleansKeyProvider<TCommand>();
//        }
//    }
//}
