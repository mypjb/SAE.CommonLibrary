using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class SaeException : Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public StatusCode Code { get; }
        /// <summary>
        /// 
        /// </summary>
        public SaeException()
        {
            this.Code = StatusCode.Custom;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public SaeException(StatusCode code) : this(code.Display())
        {
            this.Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public SaeException(StatusCode code, string message) : this(message)
        {
            this.Code = code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public SaeException(ResponseResult response) : this(response.StatusCode, response.Message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="exception"></param>
        public SaeException(StatusCode code, Exception exception) : this(code, code.Display(), exception)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public SaeException(StatusCode code, string message, Exception exception) : this(message, exception)
        {
            this.Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SaeException(string message) : base(message)
        {
            this.Code = StatusCode.Custom;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SaeException(string message, Exception innerException) : base(message, innerException)
        {
            this.Code = StatusCode.Custom;
        }
    }
}
