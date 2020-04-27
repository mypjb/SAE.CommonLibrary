using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{

    /// <summary>
    /// 
    /// </summary>
    public static class MediatorExtension
    {
        public static async Task<TResponse> Send<TResponse>(this IMediator mediator, object command)
        {
            return (TResponse)(await mediator.Send(command, typeof(TResponse)));
        }
    }
}
