using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    public class UpdateCachingPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging _logging;
        private readonly ICacheIdentityService _cacheIdentityService;

        public UpdateCachingPipelineBehavior(IDistributedCache distributedCache,
                                             ILogging<UpdateCachingPipelineBehavior<TCommand, TResponse>> logging,
                                             ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable update caching PipelineBehavior");
        }

        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            var key = this._cacheIdentityService.GetKey(command);
            this._logging.Debug($"Ready update cache '{key}'");
            var response = await next();
            await this._distributedCache.AddAsync(key, response);
            this._logging.Debug($"Cache update '{key}' success");
            return response;
        }
    }
}
