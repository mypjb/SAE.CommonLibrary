using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 缓存更新管道
    /// </summary>
    /// <remarks>
    /// 管道执行成功后，会更新缓存
    /// </remarks>
    /// <typeparam name="TCommand">命令类型</typeparam>
    /// <typeparam name="TResponse">响应类型</typeparam>
    public class UpdateCachingPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging _logging;
        private readonly ICacheIdentityService _cacheIdentityService;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="distributedCache">分布式缓存</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="cacheIdentityService">缓存标识服务</param>
        public UpdateCachingPipelineBehavior(IDistributedCache distributedCache,
                                             ILogging<UpdateCachingPipelineBehavior<TCommand, TResponse>> logging,
                                             ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable update caching PipelineBehavior");
        }
        /// <inheritdoc/>
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
