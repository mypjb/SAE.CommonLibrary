using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.AspNetCore.Scope
{
    /// <summary>
    /// aspnetcore租户策略
    /// </summary>
    public enum MultiTenantStrategy
    {
        /// <summary>
        /// 使用父子级域名
        /// </summary>
        Domain,
        /// <summary>
        /// 使用用户标识
        /// </summary>
        User,
        /// <summary>
        /// 使用请求头
        /// </summary>
        Header
    }

    /// <summary>
    /// aspnetcore多租户配置
    /// </summary>
    public class MultiTenantOptions
    {
        /// <summary>
        /// 配置项
        /// </summary>
        public const string Option = "MultiTenant";
        /// <summary>
        /// 默认租户<c>claim</c>名称
        /// </summary>
        public const string DefaultClaimName = "siteid";
        /// <summary>
        /// 默认租户请求头名称
        /// </summary>
        public const string DefaultHeaderName = "tenant";
        /// <summary>
        /// ctor
        /// </summary>
        public MultiTenantOptions()
        {
            this.Mapper = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.ClaimName = DefaultClaimName;
            this.HeaderName = DefaultHeaderName;
        }

        private string claimName;
        /// <summary>
        /// 租户<c>claim</c>名称
        /// </summary>
        /// <value></value>
        public string ClaimName
        {
            get => this.claimName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                this.claimName = value;
            }
        }

        private string headerName;
        /// <summary>
        /// 租户<c>claim</c>名称
        /// </summary>
        /// <value></value>
        public string HeaderName
        {
            get => this.headerName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                this.headerName = value;
            }
        }

        /// <summary>
        /// 租户请求头名称
        /// </summary>
        public MultiTenantStrategy Strategy { get; set; }

        /// <summary>
        /// 租户的父级域名，只有使用<see cref="MultiTenantStrategy.Domain"/>策略才有意义
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 是否使用默认策略
        /// </summary>
        public bool UseDefaultRule { get; set; }
        /// <summary>
        /// 租户域名策略映射器
        /// </summary>
        public Dictionary<string, string> Mapper { get; set; }
    }
}
