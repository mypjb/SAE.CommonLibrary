using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.Framework.Mediator.Orleans
{
    public class SiloService : ISiloService
    {
        private readonly IHost _host;

        public SiloService(IHost host)
        {
            this._host = host;
        }
        public async Task StartAsync()
        {
            await this._host.StartAsync();
        }

        public Task StopAsync()
        {
            return this._host.StopAsync();
        }
    }
}
