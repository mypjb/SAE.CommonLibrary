using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Display(Name = "成功")]
        Success = 0,
        /// <summary>
        /// 未知的异常
        /// </summary>
        [Display(Name = "未知的异常")]
        Unknown = -1,
        /// <summary>
        /// 自定义的异常
        /// </summary>
        [Display(Name = "自定义的异常")]
        Custom = -2,
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        [Display(Name = "账号或密码错误")]
        AccountOrPassword = 10001,
        /// <summary>
        /// 请求无效
        /// </summary>
        [Display(Name = "请求无效")]
        RequestInvalid = 10002,
        /// <summary>
        /// 参数无效
        /// </summary>
        [Display(Name = "参数无效")]
        ParamesterInvalid = 10003,
        /// <summary>
        /// 资源不存在
        /// </summary>
        [Display(Name = "资源不存在")]
        ResourcesNotExist = 1004,
        [Display(Name = "资源存在")]
        ResourcesExist =1005
    }
}
