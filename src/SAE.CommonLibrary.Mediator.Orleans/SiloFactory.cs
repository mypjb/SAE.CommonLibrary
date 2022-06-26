using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator;
using System.Reflection;
using Microsoft.Extensions.Logging;
using org.apache.zookeeper;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class SiloFactory : ISiloFactory
    {
        private readonly ConcurrentDictionary<string, ISiloService> _dictionary;
        private readonly ILogging<SiloFactory> _logging;
        private readonly ILoggingFactory _loggingFactory;
        private readonly IMediator _mediator;

        public SiloFactory(IOptions<OrleansOptions> options,
                           ILogging<SiloFactory> logging,
                           ILoggingFactory loggingFactory,
                           IHostEnvironment hostingEnvironment,
                           IMediator mediator)
        {
            this._dictionary = new ConcurrentDictionary<string, ISiloService>();

            this._logging = logging;
            this._loggingFactory = loggingFactory;
            this._mediator = mediator;
            if (hostingEnvironment.EnvironmentName == Environments.Development)
            {
                this.DevelopmentConfiguration(options.Value).GetAwaiter().GetResult();
            }
            else
            {
                this.ProductionConfiguration(options.Value).GetAwaiter().GetResult();
            }

        }

        private async Task DevelopmentConfiguration(OrleansOptions options)
        {
            this._logging.Warn("使用开发环境配置Silo");

            var ports = Enumerable.Range(Constants.MasterSiloPort,
                                         Constants.MasterGatewayPort - Constants.MasterSiloPort - 1)
                                  .ToList();

            await options.GrainNames.ForEachAsync(async kv =>
            {
                this._logging.Info($"创建'{options.ClusterId}'-'{kv.Key}'筒仓服务");

                var points = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

                var index = ports.FindIndex(s => !points.Any(p => p.Port == s));

                while (true)
                {
                    try
                    {
                        var siloPort = Constants.MasterSiloPort + index;
                        var gatewayPort = Constants.MasterGatewayPort + index;
                        var iPEndPoint = gatewayPort == Constants.MasterGatewayPort ? null : new IPEndPoint(IPAddress.Loopback, Constants.MasterSiloPort);
                        this._logging.Info($"筒仓端口'{siloPort}',网关端口:'{gatewayPort}'");

                        var silo = this.DefaultConfiguration(options)
                                    .UseLocalhostClustering(siloPort,
                                                            gatewayPort,
                                                            iPEndPoint)
                                    .Configure<ClusterOptions>(clusterOptions =>
                                    {
                                        clusterOptions.ClusterId = options.ClusterId;
                                        clusterOptions.ServiceId = kv.Key.ToLower();
                                    })
                                    .ConfigureLogging(configure => configure.SetMinimumLevel(LogLevel.Debug))
                                    .Build();

                        var siloService = new SiloService(silo);

                        await siloService.StartAsync();

                        this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                        this._dictionary.TryAdd(kv.Key, siloService);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logging.Error($"端口{Constants.MasterSiloPort + index}已被监听重新选择端口'{ex.Message}'", ex);
                        index++;
                    }

                }
            });
        }

        private async Task ProductionConfiguration(OrleansOptions options)
        {
            this._logging.Info("使用生产环境配置Silo");

            await options.GrainNames.ForEachAsync(async kv =>
            {
                this._logging.Info($"创建'{options.ClusterId}'-'{kv.Key}'筒仓服务");
                var silo = this.DefaultConfiguration(options)
                               .UseZooKeeperClustering(zooKeeperOptions =>
                               {
                                   zooKeeperOptions.ConnectionString = options.ZooKeeperConnectionString;
                               })
                               .Configure<ClusterOptions>(clusterOptions =>
                               {
                                   clusterOptions.ClusterId = options.ClusterId;
                                   clusterOptions.ServiceId = kv.Key.ToLower();
                               })
                               .ConfigureLogging(configure => configure.SetMinimumLevel(LogLevel.Information))
                               .Build();

                var siloService = new SiloService(silo);

                await siloService.StartAsync();

                this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                this._dictionary.TryAdd(kv.Key, siloService);
            });
        }

        private ISiloHostBuilder DefaultConfiguration(OrleansOptions options)
        {
            var silo = new SiloHostBuilder()
                             .ConfigureApplicationParts(part =>
                             {
                                 part.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences();
                             })
                             .ConfigureServices(service =>
                             {
                                 service.AddSingleton<ILoggingFactory>(this._loggingFactory)
                                        .AddMicrosoftLogging()
                                        .AddSingleton(this._mediator);
                             });

            return silo;
        }
        public IEnumerable<ISiloService> AsEnumerable()
        {
            return this._dictionary.Values;
        }

        public ISiloService Get(Type type)
        {
            ISiloService siloService = null;

            if (type != null)
            {
                var key = Utility.GetIdentity(type);

                this._dictionary.TryGetValue(key, out siloService);
            }

            return siloService;
        }
    }
}
