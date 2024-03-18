using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.AspNetCore.Scope
{
    /// <summary>
    /// aspnetcore租户策略
    /// </summary>
    public enum MultiTenantStrategy
    {
        ///Adopt parent-child domain name strategy
        Domain,
        ///Based on user information strategy
        User,
        /// Use request header
        Header
    }

    /// <summary>
    /// aspnetcore租户配置
    /// </summary>
    public class MultiTenantOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = "MultiTenant";
        /// <summary>
        /// 用户默认claim名称
        /// </summary>
        public const string DefaultClaimName = "siteid";
        /// <summary>
        /// 默认请求头名称
        /// </summary>
        public const string DefalutHeaderName = "tenant";
        /// <summary>
        /// ctor
        /// </summary>
        public MultiTenantOptions()
        {
            this.Mapper = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.ClaimName = DefaultClaimName;
            this.HeaderName = DefalutHeaderName;
        }

        private string claimName;
        /// <summary>
        /// 用户claim名称
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
        /// 请求头名称
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
        /// 多租户策略
        /// </summary>
        /// <value></value>
        public MultiTenantStrategy Strategy { get; set; }

        /// <summary>
        /// 中域名，只有在策略为<see cref="MultiTenantStrategy.Domain"/>时才生效
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 是否使用默认规则
        /// </summary>
        public bool UseDefaultRule { get; set; }
        /// <summary>
        /// 域名和租户身份的映射字典
        /// </summary>
        public Dictionary<string, string> Mapper { get; set; }
    }
}
