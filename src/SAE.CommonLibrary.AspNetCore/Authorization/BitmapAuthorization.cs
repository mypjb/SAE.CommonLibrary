using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorization : IBitmapAuthorization
    {
        private readonly ILogging<BitmapAuthorization> _logging;

        public BitmapAuthorization(ILogging<BitmapAuthorization> logging)
        {
            this._logging = logging;
        }
        public bool Authorizate(string code, int index)
        {
            var authorizate = false;

            if (index >= 0)
            {
                var bitIndex = (int)Math.Ceiling(index * 1.0 / Constants.PermissionBitsMaxPow) - 1;

                if (code.Count() > bitIndex)
                {
                    var bit = Convert.ToUInt16(code[bitIndex]);
                    //将00000001向前推进 index % Constant.PermissionBitsMaxPow 个位
                    var bitPosition = index % Constants.PermissionBitsMaxPow;

                    var permissionBit = 1 << ((bitPosition == 0 ? Constants.PermissionBitsMaxPow : bitPosition) - 1);

                    //bit位没有变化说明匹配权限位匹配正确
                    authorizate = (bit | permissionBit) == bit;
                }
                else
                {
                    this._logging.Warn($"invalid permission bit '{code}':'{index}'");
                }
            }

            return authorizate;
        }

        public string GeneratePermissionCode(IEnumerable<int> permissionBits)
        {
            if (!permissionBits.Any()) return string.Empty;

            StringBuilder sb = new StringBuilder();

            var max = permissionBits.Max();

            for (int i = 1; i <= max; i++)
            {
                sb.Append(permissionBits.Contains(i) ? '1' : '0');
            }

            var binaryDigit = sb.ToString();

            var codeStringBuilder = new StringBuilder();
            do
            {
                var index = binaryDigit.Length > Constants.PermissionBitsMaxPow ?
                                                 Constants.PermissionBitsMaxPow :
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
