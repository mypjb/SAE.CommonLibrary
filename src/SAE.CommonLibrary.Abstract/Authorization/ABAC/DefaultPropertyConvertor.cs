using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IPropertyConvertor{T}"/>默认实现
    /// </summary>
    public class DefaultPropertyConvertor : IPropertyConvertor<bool>,
                                            IPropertyConvertor<float>,
                                            IPropertyConvertor<DateTime>,
                                            IPropertyConvertor<TimeSpan>,
                                            IPropertyConvertor<string>
    {
        /// <summary>
        /// 日至记录器
        /// </summary>
        private readonly ILogging _logging;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging">日志记录器</param>
        public DefaultPropertyConvertor(ILogging<DefaultPropertyConvertor> logging)
        {
            this._logging = logging;
        }
        ///<inheritdoc/>
        bool IPropertyConvertor<bool>.Convert(string val)
        {
            bool result;

            if (string.IsNullOrWhiteSpace(val) || !bool.TryParse(val, out result))
            {
                this._logging.Warn($"{val} -> bool 转换失败");
                result = false;
            }

            return result;
        }
        ///<inheritdoc/>
        float IPropertyConvertor<float>.Convert(string val)
        {
            float result;

            if (string.IsNullOrWhiteSpace(val) || !float.TryParse(val, out result))
            {
                this._logging.Warn($"{val} -> float 转换失败");
                result = 0f;
            }

            return result;
        }
        ///<inheritdoc/>
        DateTime IPropertyConvertor<DateTime>.Convert(string val)
        {
            DateTime result;

            if (string.IsNullOrWhiteSpace(val) || !DateTime.TryParse(val, out result))
            {
                this._logging.Warn($"{val} -> DateTime 转换失败");
                result = DateTime.MinValue;
            }

            return result;
        }
        ///<inheritdoc/>
        TimeSpan IPropertyConvertor<TimeSpan>.Convert(string val)
        {
            TimeSpan result;

            if (string.IsNullOrWhiteSpace(val) || !TimeSpan.TryParse(val, out result))
            {
                this._logging.Warn($"{val} -> TimeSpan 转换失败");
                result = TimeSpan.Zero;
            }

            return result;
        }
        ///<inheritdoc/>
        string IPropertyConvertor<string>.Convert(string val)
        {
            return string.IsNullOrWhiteSpace(val) ? string.Empty : val.Trim();
        }
    }
}