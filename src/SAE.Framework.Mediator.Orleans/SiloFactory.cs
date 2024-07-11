using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using SAE.Framework.Abstract.Mediator;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.Mediator.Orleans
{
    public class SiloFactory : ISiloFactory
    {
        private readonly ConcurrentDictionary<string, IHost> _dictionary;
        private readonly ILogging<SiloFactory> _logging;
        private readonly ILoggingFactory _loggingFactory;
        private readonly IMediator _mediator;

        public SiloFactory(IOptions<OrleansOptions> options,
                           ILogging<SiloFactory> logging,
                           ILoggingFactory loggingFactory,
                           IHostEnvironment hostingEnvironment,
                           IMediator mediator)
        {
            this._dictionary = new ConcurrentDictionary<string, IHost>();

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

                        var host = this.DefaultConfiguration(builder =>
                        {
                            builder.UseLocalhostClustering(siloPort,
                                                            gatewayPort,
                                                            iPEndPoint)
                                    .Configure<ClusterOptions>(clusterOptions =>
                                    {
                                        clusterOptions.ClusterId = options.ClusterId;
                                        clusterOptions.ServiceId = kv.Key.ToLower();
                                    })
                                    .ConfigureLogging(configure => configure.SetMinimumLevel(LogLevel.Debug));
                        });

                        await host.StartAsync();

                        this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                        this._dictionary.TryAdd(kv.Key, host);
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
                var host = this.DefaultConfiguration(builder =>
                {
                    builder.Configure<ClusterOptions>(clusterOptions =>
                           {
                               clusterOptions.ClusterId = options.ClusterId;
                               clusterOptions.ServiceId = kv.Key.ToLower();
                           })
                           .ConfigureLogging(configure => configure.SetMinimumLevel(LogLevel.Information));
                });

                await host.StartAsync();

                this._logging.Info($"筒仓'{options.ClusterId}'-'{kv.Key}'已启动");

                this._dictionary.TryAdd(kv.Key, host);
            });
        }

        private IHost DefaultConfiguration(Action<ISiloBuilder> action)
        {

            var silo = new HostBuilder()
                            .UseOrleans(builder =>
                            {
                                builder.ConfigureServices(service =>
                                  {
                                      service.AddSingleton<ILoggingFactory>(this._loggingFactory)
                                             .AddSingleton(this._mediator)
                                             .AddSAEFramework()
                                             .AddMicrosoftLogging();
                                  });
                                action(builder);
                            });

            return silo.Build();
        }
        public IEnumerable<IHost> AsEnumerable()
        {
            return this._dictionary.Values;
        }

        public IHost Get(Type type)
        {
            IHost host = null;

            if (type != null)
            {
                var key = Utility.GetIdentity(type);

                this._dictionary.TryGetValue(key, out host);
            }

            return host;
        }
    }
}
