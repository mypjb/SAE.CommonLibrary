using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 位图授权处理程序
    /// </summary>
    /// <inheritdoc/>
    public class BitmapAuthorizationHandler : AuthorizationHandler<BitmapAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBitmapEndpointStorage _bitmapEndpointStorage;
        private readonly IBitmapAuthorization _bitmapAuthorization;
        private readonly ILogging _logging;
        private readonly ICache _cache;
        private readonly IOptionsSnapshot<List<BitmapAuthorizationDescriptor>> _optionsSnapshot;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="httpContextAccessor">请求上下文</param>
        /// <param name="bitmapEndpointStorage">端点存储器</param>
        /// <param name="bitmapAuthorization">位图授权规则对象</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="cache">内部缓存</param>
        /// <param name="optionsSnapshot">配置</param>
        public BitmapAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
                                          IBitmapEndpointStorage bitmapEndpointStorage,
                                          IBitmapAuthorization bitmapAuthorization,
                                          ILogging<BitmapAuthorization> logging,
                                          IDistributedCache cache,
                                          IOptionsSnapshot<List<BitmapAuthorizationDescriptor>> optionsSnapshot)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._bitmapEndpointStorage = bitmapEndpointStorage;
            this._bitmapAuthorization = bitmapAuthorization;
            this._logging = logging;
            this._cache = cache;
            this._optionsSnapshot = optionsSnapshot;
        }
        /// <summary>
        /// 验证当前认证标识是否和符合位图授权标准
        /// </summary>
        /// <param name="context">验证上下文</param>
        /// <param name="requirement">授权需求</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BitmapAuthorizationRequirement requirement)
        {
            if (!context.User.FindFirst(Constants.BitmapAuthorize.Administrator)?.Value.IsNullOrWhiteSpace() ?? false)//是否是超管
            {
                context.Succeed(requirement);
            }
            else
            {
                this.AuthorizeCore(context, requirement);
            }


            return Task.CompletedTask;
        }

        protected virtual void AuthorizeCore(AuthorizationHandlerContext context, BitmapAuthorizationRequirement requirement)
        {
            var ctx = this._httpContextAccessor.HttpContext;

            var index = this._bitmapEndpointStorage.GetIndex(ctx);
            string code = string.Empty;
            if (context.User.Identity.IsAuthenticated && index > 0)
            {
                var claims = context.User.FindAll(Constants.BitmapAuthorize.Claim) ?? Enumerable.Empty<Claim>();

                code = this._bitmapAuthorization.FindCode(claims);

                var bitmapAuthorizations = this.GetAuthorizeDescriptor(code);

                foreach (var bitmapAuthorization in bitmapAuthorizations)
                {
                    if (this._bitmapAuthorization.Authorize(bitmapAuthorization.Code, index))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
            else if (index == 0)
            {
                this._logging.Info("索引为0,默认授权访问");
                //index not exist default auth
                context.Succeed(requirement);
            }

            var message = $"url:'{ctx.Request.GetDisplayUrl()}',method:{ctx.Request.Method},index:{index},code:'{code}'";
            if (context.HasSucceeded)
            {
                this._logging.Debug($"本次请求，授权成功！{message}");
            }
            else
            {
                this._logging.Debug($"本次请求，授权失败！{message}");
            }
        }


        /// <summary>
        /// 获得授权描述符，如果获取失败，会删除缓存并重试<see cref="Constants.BitmapAuthorize.MaxFindNum"/>次
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="num">当前重试次数</param>
        protected virtual IEnumerable<BitmapAuthorizationDescriptor> GetAuthorizeDescriptor(string code, int num = 0)
        {
            var cacheKey = $"{Constants.BitmapAuthorize.Caching.AuthorizeCode}{code.ToMd5()}";

            this._logging.Debug("对授权码进行md5值运算");

            var descriptors = this._optionsSnapshot.Value.ToArray();

            var indexs = this._cache.GetOrAdd<int[]>(cacheKey, () =>
            {
                return descriptors.Where(s => this._bitmapAuthorization.Authorize(code, s.Index))
                                   .Select(s => s.Index)
                                   .ToArray();
            }, Constants.BitmapAuthorize.Caching.AuthorizeCodeTime);

            var max = indexs.Any() ? indexs.Max() : 0;

            if (descriptors.Length >= max)
            {
                return descriptors.Where((desc, index) =>
                {
                    return indexs.Contains(desc.Index);
                }).ToArray();
            }
            else
            {
                num++;
                var message = $"索引最大长度为：'{max}',当前权限位最大长度:'{descriptors.Length}',已查找次数:'{num}'";
                if (num >= Constants.BitmapAuthorize.MaxFindNum)
                {
                    this._logging.Error($"查找授权码失败，并超过了最大次数'{Constants.BitmapAuthorize.MaxFindNum}'！{message}");
                    return Enumerable.Empty<BitmapAuthorizationDescriptor>();
                }
                else
                {

                    this._logging.Warn($"查找授权码失败，该问题是由于两边数据不匹配造成的!{message}");
                    this._cache.Delete(cacheKey);
                    return this.GetAuthorizeDescriptor(code, num);
                }

            }
        }
    }
}
