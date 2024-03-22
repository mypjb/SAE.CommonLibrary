using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    ///  AspNetCore 授权描述符<seealso cref="AuthDescriptor"/>
    /// </summary>
    /// <inheritdoc/>
    public class AspNetCoreAuthDescriptor : AuthDescriptor
    {
        /// <summary>
        /// 访问路径
        /// </summary>
        /// <value></value>
        public string Path { get; set; }
        /// <summary>
        /// 请求谓词
        /// </summary> 
        public string Method { get; set; }
        
        /// <inheritdoc/>
        public override string Key
        {
            get
            {
                return $"{this.Method}:{Path}".ToLower();
            }
            set
            {
            }
        }

        /// <summary>
        /// <see cref="Key"/>的匹配模式
        /// </summary>
        /// <remarks>
        /// <para>
        /// 0：使用基类<see cref="AuthDescriptor"/>的匹配规则。
        /// </para>
        /// <para>
        /// 1：使用正则进行匹配。
        /// </para>
        /// </remarks>
        public int MatchPattern { get; set; }

        /// <inheritdoc/>
        public override bool Comparison(string key)
        {
            if (MatchPattern == 1)
            {
                return Regex.Match(key, this.Key).Success;
            }

            return base.Comparison(key);
        }
    }
}