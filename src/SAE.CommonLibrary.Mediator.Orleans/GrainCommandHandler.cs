using Orleans;
using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class GrainCommandHandler : Grain, IGrainCommandHandler
    {
        private readonly IMediator _mediator;

        public GrainCommandHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }


        public Task<object> Send(object @object, Type commandType, Type responseType)
        {
            return this._mediator.Send(@object, commandType, responseType);
        }

        public Task Send(object @object, Type commandType)
        {
            return this._mediator.Send(@object, commandType);
        }
    }
}
