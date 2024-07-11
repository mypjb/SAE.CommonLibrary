using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework
{
    /// <summary>
    /// 错误输出
    /// </summary>
    public class ErrorOutput
    {
        /// <summary>
        /// 清使用其他对象进行实例化
        /// </summary>
        [Obsolete("清使用其他对象进行实例化", false)]
        public ErrorOutput()
        {

        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        public ErrorOutput(StatusCodes code) : this(code, code.GetDetail())
        {
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">输出信息</param>
        public ErrorOutput(int code, string message)
        {
            Assert.Build(message).NotNullOrWhiteSpace();

            this.StatusCode = code;
            this.Message = message;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">输出信息</param>
        public ErrorOutput(StatusCodes code, string message) : this((int)code, message)
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="exception">异常</param>
        public ErrorOutput(Exception exception) : this(StatusCodes.Unknown, exception)
        {

        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="exception">异常</param>
        public ErrorOutput(SAEException exception) : this(exception.Code, exception)
        {

        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="exception">异常</param>
        public ErrorOutput(StatusCodes code, Exception exception) : this(code, exception?.Message)
        {
        }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="exception">异常</param>
        public ErrorOutput(int code, Exception exception) : this(code, exception?.Message)
        {
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message
        {
            get; set;
        }
    }
}
