using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 默认的<see cref="IBitmapAuthorization"/>实现
    /// </summary>
    public class BitmapAuthorization : IBitmapAuthorization
    {
        private readonly ILogging _logging;
        private readonly IScopeFactory _scopeFactory;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="scopeFactory">区域工厂</param>
        public BitmapAuthorization(
            ILogging<BitmapAuthorization> logging,
            IScopeFactory scopeFactory)
        {
            this._logging = logging;
            this._scopeFactory = scopeFactory;
        }

        public virtual bool Authorize(string code, int index)
        {
            var authorize = false;

            if (index >= 0 && !string.IsNullOrEmpty(code))
            {
                var bitIndex = (int)Math.Ceiling(index * 1.0 / Constants.BitmapAuthorize.MaxPow) - 1;

                if (code.Length > bitIndex)
                {
                    var bit = Convert.ToUInt16(code[bitIndex]);
                    //将00000001向前推进 index % Constant.BitmapAuthorize.MaxPow 个位
                    var bitPosition = index % Constants.BitmapAuthorize.MaxPow;

                    var permissionBit = 1 << ((bitPosition == 0 ? Constants.BitmapAuthorize.MaxPow : bitPosition) - 1);

                    //bit位没有变化说明匹配权限位匹配正确
                    authorize = (bit | permissionBit) == bit;
                }
                else
                {
                    this._logging.Warn($"invalid permission bit '{code}':'{index}'");
                }
            }

            return authorize;
        }

        public virtual string FindCode(IEnumerable<Claim> claims)
        {

            string code = null;

            var name = this._scopeFactory.Get().Name;

            if (name.IsNullOrWhiteSpace())
            {
                code = claims.FirstOrDefault()?.Value;
            }
            else
            {
                var prefix = $"{name}{Constants.BitmapAuthorize.GroupSeparator}";

                foreach (var claim in claims)
                {
                    if (claim.Value.StartsWith(prefix))
                    {
                        code = claim.Value.Substring(prefix.Length);
                        break;
                    }
                }
            }

            return string.IsNullOrEmpty(code) ? string.Empty : code;
        }

        public virtual string GenerateCode(IEnumerable<int> authBits)
        {
            if (!authBits.Any()) return string.Empty;

            var bits = authBits.ToList();
            //删除无效的索引
            bits.RemoveAll(s => s < Constants.BitmapAuthorize.InitialIndex);
            //去除重复的索引
            authBits = bits.Distinct().ToArray();

            if (!authBits.Any()) return string.Empty;

            StringBuilder sb = new StringBuilder();

            var max = authBits.Max();

            for (int i = 1; i <= max; i++)
            {
                sb.Append(authBits.Contains(i) ? '1' : '0');
            }

            var binaryDigit = sb.ToString();

            var codeStringBuilder = new StringBuilder();
            do
            {
                var index = binaryDigit.Length > Constants.BitmapAuthorize.MaxPow ?
                                                 Constants.BitmapAuthorize.MaxPow :
                                                 binaryDigit.Length;

                var bit = Convert.ToUInt16(new string(binaryDigit.Substring(0, index)
                                                 .Reverse()
                                                 .ToArray()), 2);

                var pbit = Convert.ToChar(bit);

                codeStringBuilder.Append(pbit);

                binaryDigit = binaryDigit.Remove(0, index);

            } while (binaryDigit.Length > 0);

            return codeStringBuilder.ToString();
        }
    }
}
