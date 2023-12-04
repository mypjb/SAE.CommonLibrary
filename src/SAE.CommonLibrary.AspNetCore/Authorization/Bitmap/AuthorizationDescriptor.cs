using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// bitmap 授权描述符
    /// </summary>
    public class AuthorizationDescriptor
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = Constants.Authorize.Option + ":bitmaps";
        /// <summary>
        /// bitmap索引，从1开始
        /// </summary>
        /// <value></value>
        public int Index { get; set; }
        /// <summary>
        /// bitmap名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
        /// <summary>
        /// bitmap 授权码
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
    }
}