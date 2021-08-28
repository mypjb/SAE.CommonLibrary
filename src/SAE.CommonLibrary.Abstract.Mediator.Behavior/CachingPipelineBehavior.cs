using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    public class CachingPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging<CachingPipelineBehavior<TCommand, TResponse>> _logging;

        public CachingPipelineBehavior(IDistributedCache distributedCache,
                                       ILogging<CachingPipelineBehavior<TCommand,TResponse>> logging)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._logging.Info($"Enable Caching PipelineBehavior");
        }
        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            this._logging.Debug("Entry caching pipeline");
            var response = await this._distributedCache.GetOrAddAsync(command.ToString(), next);
            this._logging.Debug("End caching pipeline");
            return response;
        }
    }
}
