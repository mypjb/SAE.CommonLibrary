using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.AspNetCore.Scope
{
    /// <summary>
    /// aspnetcore�⻧����
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
    /// aspnetcore�⻧����
    /// </summary>
    public class MultiTenantOptions
    {
        /// <summary>
        /// ���ý�
        /// </summary>
        public const string Option = "MultiTenant";
        /// <summary>
        /// �û�Ĭ��claim����
        /// </summary>
        public const string DefaultClaimName = "siteid";
        /// <summary>
        /// Ĭ������ͷ����
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
        /// �û�claim����
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
        /// ����ͷ����
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
        /// ���⻧����
        /// </summary>
        /// <value></value>
        public MultiTenantStrategy Strategy { get; set; }

        /// <summary>
        /// ��������ֻ���ڲ���Ϊ<see cref="MultiTenantStrategy.Domain"/>ʱ����Ч
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// �Ƿ�ʹ��Ĭ�Ϲ���
        /// </summary>
        public bool UseDefaultRule { get; set; }
        /// <summary>
        /// �������⻧��ݵ�ӳ���ֵ�
        /// </summary>
        public Dictionary<string, string> Mapper { get; set; }
    }
}
