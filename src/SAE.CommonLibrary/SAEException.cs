using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 内部异常
    /// </summary>
    public class SAEException : Exception
    {
        /// <summary>
        /// 错误码(状态码)
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// 实例化
        /// </summary>
        public SAEException() : this(StatusCodes.Custom, StatusCodes.Custom.GetDetail())
        {
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        public SAEException(StatusCodes code) : this(code, code.GetDetail())
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">c错误信息</param>
        public SAEException(StatusCodes code, string message) : this((int)code, message)
        {
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">错误信息</param>
        public SAEException(int code, string message) : base(message)
        {
            this.Code = code;
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="error">错误输出</param>
        public SAEException(ErrorOutput error) : this(error.StatusCode, error.Message)
        {

        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="message">错误信息</param>
        public SAEException(string message) : this(StatusCodes.Custom, message)
        {
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="innerException">内部异常</param>
        public SAEException(string message, Exception innerException) : this(StatusCodes.Custom, message, innerException)
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="exception">异常</param>
        public SAEException(StatusCodes code, Exception exception) : this(code, code.GetDetail(), exception)
        {

        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">错误信息</param>
        /// <param name="exception">异常</param>
        public SAEException(StatusCodes code, string message, Exception exception) : this((int)code, message, exception)
        {
            this.Code = (int)code;
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">错误信息</param>
        /// <param name="exception">异常</param>
        public SAEException(int code, string message, Exception exception) : base(message, exception)
        {
            this.Code = code;
        }
    }
}
