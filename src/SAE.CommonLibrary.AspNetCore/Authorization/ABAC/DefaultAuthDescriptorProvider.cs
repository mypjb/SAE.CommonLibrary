using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IAuthDescriptorProvider"/>默认实现
    /// </summary>
    public class DefaultABACAuthDescriptorProvider : IAuthDescriptorProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogging _logging;
        private IEnumerable<AuthDescriptor> authDescriptors;

        public DefaultABACAuthDescriptorProvider(IHttpContextAccessor httpContextAccessor,
                                                 IOptionsMonitor<AuthOptions> optionsMonitor,
                                                 IMemoryCache memoryCache,
                                                 ILogging<DefaultABACAuthDescriptorProvider> logging)
        {
            this._memoryCache = memoryCache;
            this._logging = logging;
            this._httpContextAccessor = httpContextAccessor;
            optionsMonitor.OnChange(this.SetAuthDescriptor);
            this.SetAuthDescriptor(optionsMonitor.CurrentValue);
        }

        private void SetAuthDescriptor(AuthOptions options)
        {
            this.authDescriptors = options?.Descriptors ?? new AuthDescriptor[0];
            this._logging.Info($"加载ABAC授权规则：{this.authDescriptors.Count()}条");
            this._logging.Info("清空缓存信息。");
            this._memoryCache.Clear();
        }
        public async Task<AuthDescriptor> GetAsync()
        {
            var ctx = this._httpContextAccessor.HttpContext;

            string path = ctx.Request.Path == Constants.Request.EndingSymbol.ToString() ?
                         ctx.Request.Path :
                         ctx.Request.Path.ToString().TrimEnd(Constants.Request.EndingSymbol);

            var key = $"{ctx.Request.Method}:{path}".ToLower();

            this._logging.Info($"查找授权描述符：{key}");

            var authDescriptor = await this._memoryCache.GetOrAddAsync<AuthDescriptor>(key, async () =>
            {
                this._logging.Info($"缓存尚未命中，重新进行初始化操作，并加入缓存:{key}");
                return this.authDescriptors.FirstOrDefault(s => s.Path == path &&
                                                           s.Method.IndexOf(ctx.Request.Method, StringComparison.OrdinalIgnoreCase) != -1);
            });

            return authDescriptor;
        }
    }
}