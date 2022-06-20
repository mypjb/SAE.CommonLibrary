using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    public class DeleteCachingPipelineBehavior<TCommand> : IPipelineBehavior<TCommand> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging _logging;
        private readonly ICacheIdentityService _cacheIdentityService;

        public DeleteCachingPipelineBehavior(IDistributedCache distributedCache,
                                             ILogging<DeleteCachingPipelineBehavior<TCommand>> logging,
                                             ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable delete caching PipelineBehavior");
        }
        public async Task ExecutionAsync(TCommand command, Func<Task> next)
        {
            var key = this._cacheIdentityService.GetKey(command);
            this._logging.Debug($"Ready delete cache '{key}'");
            await next();
            await this._distributedCache.DeleteAsync(key);
            this._logging.Debug($"Cache delete '{key}' success");
        }
    }

    public class DeleteCachingPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging _logging;
        private readonly ICacheIdentityService _cacheIdentityService;

        public DeleteCachingPipelineBehavior(IDistributedCache distributedCache,
                                             ILogging<DeleteCachingPipelineBehavior<TCommand, TResponse>> logging,
                                             ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable delete caching PipelineBehavior");
        }

        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            var key = this._cacheIdentityService.GetKey(command);
            this._logging.Debug($"Ready delete cache '{key}'");
            var response = await next();
            await this._distributedCache.DeleteAsync(key);
            this._logging.Debug($"Cache delete '{key}' success");
            return response;
        }
    }
}
