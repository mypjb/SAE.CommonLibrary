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
        Task SendAsync(object command,Type commandType);
        Task<object> SendAsync(object command,Type commandType,Type responseType);
    }
}
