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

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class SiloFactory : ISiloFactory
    {
        private readonly ConcurrentDictionary<string, ISiloService> _dictionary;
        private readonly ILogging<SiloFactory> _logging;
        private readonly IMediator _mediator;

        public SiloFactory(OrleansOptions options, 
                           ILogging<SiloFactory> logging,
                           IHostEnvironment hostingEnvironment,
                           IMediator mediator)
        {
            this._dictionary = new ConcurrentDictionary<string, ISiloService>();

            this._logging = logging;
            this._mediator = mediator;
            if (hostingEnvironment.EnvironmentName == Environments.Development)
            {
                this.DevelopmentConfiguration(options).GetAwaiter().GetResult();
            }
            else
            {
                this.ProductionConfiguration(options).GetAwaiter().GetResult();
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
                        var silo = new SiloHostBuilder()
                                    .UseLocalhostClustering(Constants.MasterSiloPort + index,
                                                            Constants.MasterGatewayPort + index,
                                                            new IPEndPoint(IPAddress.Loopback, Constants.MasterSiloPort))
                                    .Configure<ClusterOptions>(clusterOptions =>
                                    {
                                        clusterOptions.ClusterId = options.ClusterId;
                                        clusterOptions.ServiceId = kv.Key;
                                    })
                                    .ConfigureApplicationParts(part =>
                                    {
                                        part.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences();
                                    })
                                    .ConfigureServices(service =>
                                    {
                                        service.AddSingleton(this._mediator);
                                    })
                                    .Build();

                        var siloService = new SiloService(silo);

                        await siloService.StartAsync();

                        this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                        this._dictionary.TryAdd(kv.Key, siloService);
                    }
                    catch (Exception ex)
                    {
                        _logging.Error($"端口{Constants.MasterSiloPort + index}已被监听重新选择端口'{ex.Message}'", ex);
                    }
                    break;
                }
            });
        }

        private async Task ProductionConfiguration(OrleansOptions options)
        {
            this._logging.Info("使用生产环境配置Silo");

            await options.GrainNames.ForEachAsync(async kv =>
            {
                this._logging.Info($"创建'{options.ClusterId}'-'{kv.Key}'筒仓服务");
                var silo = new SiloHostBuilder()
                                    .UseZooKeeperClustering(zooKeeperOptions =>
                                    {
                                        zooKeeperOptions.ConnectionString = options.ZooKeeperConnectionString;
                                    })
                                    .Configure<ClusterOptions>(clusterOptions =>
                                    {
                                        clusterOptions.ClusterId = options.ClusterId;
                                        clusterOptions.ServiceId = kv.Key;
                                    })
                                    .ConfigureApplicationParts(part =>
                                    {
                                        part.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences();
                                    })
                                    .ConfigureServices(service =>
                                    {
                                        service.AddSingleton(this._mediator);
                                    })
                                    .Build();
                var siloService = new SiloService(silo);

                await siloService.StartAsync();

                this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                this._dictionary.TryAdd(kv.Key, siloService);
            });
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
