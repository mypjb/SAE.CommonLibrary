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
    /// 缓存管道
    /// </summary>
    /// <remarks>
    /// 命令执行成功后会缓存<typeparamref name="TResponse"/>,下次请求如果缓存存在，会优先从缓存获取。
    /// </remarks>
    /// <typeparam name="TCommand">命令类型</typeparam>
    /// <typeparam name="TResponse">响应类型</typeparam>
    public class CachingPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging<CachingPipelineBehavior<TCommand, TResponse>> _logging;
        private readonly ICacheIdentityService _cacheIdentityService;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="distributedCache">分布式缓存</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="cacheIdentityService">缓存标识服务</param>
        public CachingPipelineBehavior(IDistributedCache distributedCache,
                                       ILogging<CachingPipelineBehavior<TCommand,TResponse>> logging,
                                       ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable caching PipelineBehavior");
        }
        /// <inheritdoc/>
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
