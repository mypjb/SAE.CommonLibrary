using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class SiloService : ISiloService
    {
        private readonly ISiloHost _silo;

        public SiloService(ISiloHost silo)
        {
            this._silo = silo;
        }
        public Task StartAsync()
        {
            return this._silo.StartAsync();
        }

        public Task StopAsync()
        {
            return this._silo.StopAsync();
        }
    }
}
