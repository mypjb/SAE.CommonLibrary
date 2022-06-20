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
        private readonly ICacheIdentityService _cacheIdentityService;

        public CachingPipelineBehavior(IDistributedCache distributedCache,
                                       ILogging<CachingPipelineBehavior<TCommand,TResponse>> logging,
                                       ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable caching PipelineBehavior");
        }
        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            var key = this._cacheIdentityService.GetKey(command);
            this._logging.Debug($"Entry caching pipeline key('{key}')");
            var response = await this._distributedCache.GetOrAddAsync(key, next);
            this._logging.Debug($"End caching pipeline key('{key}')");
            return response;
        }
    }
}
