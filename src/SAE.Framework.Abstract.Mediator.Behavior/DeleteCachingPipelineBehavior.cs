using SAE.Framework.Caching;
using SAE.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 缓存删除管道
    /// </summary>
    /// <remarks>
    /// 命令执行成功会删除缓存,会删除<typeparamref name="TCommand"/>生成的缓存key
    /// </remarks>
    /// <typeparam name="TCommand">命令类型</typeparam>
    public class DeleteCachingPipelineBehavior<TCommand> : IPipelineBehavior<TCommand> where TCommand : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogging _logging;
        private readonly ICacheIdentityService _cacheIdentityService;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="distributedCache">分布式缓存接口</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="cacheIdentityService">缓存标识服务</param>
        public DeleteCachingPipelineBehavior(IDistributedCache distributedCache,
                                             ILogging<DeleteCachingPipelineBehavior<TCommand>> logging,
                                             ICacheIdentityService cacheIdentityService)
        {
            this._distributedCache = distributedCache;
            this._logging = logging;
            this._cacheIdentityService = cacheIdentityService;
            this._logging.Info($"Enable delete caching PipelineBehavior");
        }
        /// <inheritdoc/>
        public async Task ExecutionAsync(TCommand command, Func<Task> next)
        {
            var key = this._cacheIdentityService.GetKey(command);
            this._logging.Debug($"Ready delete cache '{key}'");
            await next();
            await this._distributedCache.DeleteAsync(key);
            this._logging.Debug($"Cache delete '{key}' success");
        }
    }
    /// <summary>
    /// 缓存删除管道
    /// </summary>
    /// <remarks>
    /// 命令执行成功后，会删除<typeparamref name="TCommand"/>生成的缓存key
    /// </remarks>
    /// <typeparam name="TCommand">命令类型</typeparam>
    /// <typeparam name="TResponse">响应类型</typeparam>
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
