using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public interface ISiloService
    {
        Task StartAsync();
        Task StopAsync();
    }
}
