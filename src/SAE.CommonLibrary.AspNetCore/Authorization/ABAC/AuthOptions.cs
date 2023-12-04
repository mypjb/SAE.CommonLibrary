using System.Collections.Generic;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// 授权配置
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = Constants.Authorize.Option + ":abac";
        /// <summary>
        /// 授权描述符
        /// </summary>
        /// <value></value>
        public AuthDescriptor[] Descriptors { get; set; }
    }

    /// <summary>
    /// 授权描述符
    /// </summary>
    public class AuthDescriptor
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 访问路径
        /// </summary>
        /// <value></value>
        public string Path { get; set; }
        /// <summary>
        /// 请求谓词
        /// </summary> 
        public string Method { get; set; }
        /// <summary>
        /// 授权规则
        /// </summary>
        /// <value></value>
        public string Rule { get; set; }
    }
}