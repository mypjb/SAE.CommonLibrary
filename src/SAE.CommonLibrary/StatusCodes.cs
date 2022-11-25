using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StatusCodes
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Display(Name = "成功")]
        Success = 200,
        /// <summary>
        /// 未知的异常
        /// </summary>
        [Display(Name = "内部异常,请联系工程")]
        Unknown = 500,
        Custom = 501,
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        [Display(Name = "用户或密码错误")]
        AccountOrPassword = 401,
        /// <summary>
        /// 请求无效
        /// </summary>
        [Display(Name = "请求无效")]
        RequestInvalid = 4001,
        /// <summary>
        /// 参数无效
        /// </summary>
        [Display(Name = "参数无效")]
        ParamesterInvalid = 4002,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "资源存在")]
        ResourcesExist = 4003,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "版本号不一致")]
        ResourcesVersion = 4004,
        /// <summary>
        /// 资源不存在
        /// </summary>
        [Display(Name = "资源不存在")]
        ResourcesNotExist = 404,

    }
}
