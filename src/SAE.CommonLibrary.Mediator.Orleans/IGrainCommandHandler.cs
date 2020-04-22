using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public interface IGrainCommandHandler:IGrainWithStringKey
    {
        Task Send(object @object);
        Task<object> Send(object @object,Type responseType);
    }
}
