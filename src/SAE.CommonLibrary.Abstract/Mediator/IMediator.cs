using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 中介者
    /// </summary>
    public interface IMediator
    {
        Task Send(object command);
        Task<TResponse> Send<TResponse>(object command);
    }
}
