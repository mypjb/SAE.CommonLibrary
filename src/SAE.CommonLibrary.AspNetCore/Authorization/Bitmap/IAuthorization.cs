using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// 基于位图的授权规则
    /// </summary>
    public interface IAuthorization
    {
        /// <summary>
        /// 根据<paramref name="authBits"/>生成授权码
        /// </summary>
        /// <param name="authBits">授权位集合</param>
        /// <returns>授权码</returns>
        string GenerateCode(IEnumerable<int> authBits);
        /// <summary>
        /// 判断<paramref name="index"/>是否存在<paramref name="code"/>当中，
        /// 如果存在则返回<code>true</code>否侧<code>false</code>
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="index">权限索引</param>
        /// <returns>返回<code>true</code>授权成功反之失败</returns>
        bool Authorize(string code, int index);
        /// <summary>
        /// 查找授权码
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string FindCode(IEnumerable<Claim> claims);

    }
}
