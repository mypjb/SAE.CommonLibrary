using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}