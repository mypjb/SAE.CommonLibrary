using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorizationOptions
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 权限组，用于从上下文中匹配权限码
        /// </summary>
        public string PermissionGroup { get; set; }
    }
}
