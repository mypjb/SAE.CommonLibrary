using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorization : IBitmapAuthorization
    {
        public bool Authorizate(string code, int index)
        {
            --index;

            var authorizate = false;

            if (index >=0)
            {
                var bitIndex = index / Constant.PermissionBitsMaxPow;

                if (code.Count() >= bitIndex)
                {
                    var bit = Encoding.UTF8.GetBytes(code[bitIndex].ToString()).First();

                    //将1向前推进 index % Constant.PermissionBitsMaxPow 个位
                    var bitPosition = 1 << (index % Constant.PermissionBitsMaxPow);

                    //bit位没有变化说明匹配权限位匹配正确
                    authorizate = (bit | bitPosition) == bit;
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

            var bytes = new List<byte>();
            do
            {
                var index = binaryDigit.Length > Constant.PermissionBitsMaxPow ?
                                                 Constant.PermissionBitsMaxPow :
                                                 binaryDigit.Length;

                bytes.Add(Convert.ToByte(new string(binaryDigit.Substring(0, index)
                                             .Reverse()
                                             .ToArray()),
                                             2));

                binaryDigit = binaryDigit.Remove(0, index);

            } while (binaryDigit.Length > 0);

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
