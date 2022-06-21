using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Scope.AspNetCore
{
    /// <summary>
    /// aspnetcore multi tenant strategy
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
    /// aspnetcore tenant options
    /// </summary>
    public class MultiTenantOptions
    {
        /// <summary>
        /// configuration node
        /// </summary>
        public const string Option = "MultiTenant";
        /// <summary>
        /// default user claim name
        /// </summary>
        public const string DefaultClaimName = "SiteId";
        /// <summary>
        /// default header name
        /// </summary>
        public const string DefalutHeaderName = "Tenant";
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
        /// tenant identity claim name
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
        /// tenant identity claim name
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
        /// multi tenant strategy
        /// </summary>
        /// <value></value>
        public MultiTenantStrategy Strategy { get; set; }

        /// <summary>
        /// master host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// set default rule
        /// </summary>
        public bool UseDefaultRule { get; set; }
        /// <summary>
        /// mapping dictionary of host and tenant identity 
        /// </summary>
        public Dictionary<string, string> Mapper { get; set; }
    }
}
