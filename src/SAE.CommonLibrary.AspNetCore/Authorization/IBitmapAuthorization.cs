using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public interface IBitmapAuthorization
    {
        /// <summary>
        /// 根据<paramref name="permissionBits"/>生成权限代码
        /// </summary>
        /// <param name="permissionBits">权限位集合</param>
        /// <returns>权限码</returns>
        string GeneratePermissionCode(IEnumerable<int> permissionBits);
        /// <summary>
        /// 判断<paramref name="index"/>是否存在<paramref name="code"/>当中，
        /// 如果存在则返回<seealso cref="true"/>否侧<seealso cref="false"/>
        /// </summary>
        /// <param name="code">权限码</param>
        /// <param name="index">权限索引</param>
        /// <returns>返回<seealso cref="true"/>授权成功反之失败</returns>
        bool Authorizate(string code, int index);
        /// <summary>
        /// 查找权限码
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string FindPermissionCode(IEnumerable<Claim> claims);

    }
}
