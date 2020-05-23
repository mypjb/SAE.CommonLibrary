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
        public int Code { get; }
        /// <summary>
        /// 
        /// </summary>
        public SaeException() : this(StatusCodes.Custom, StatusCodes.Custom.GetDetail())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public SaeException(StatusCodes code) : this(code, code.GetDetail())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public SaeException(StatusCodes code, string message) : this((int)code, message)
        {
        }

        public SaeException(int code, string message) : base(message)
        {
            this.Code = code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public SaeException(ErrorOutput error) : this(error.StatusCode, error.Message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SaeException(string message) : this(StatusCodes.Custom, message)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SaeException(string message, Exception innerException) : this(StatusCodes.Custom, message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="exception"></param>
        public SaeException(StatusCodes code, Exception exception) : this(code, code.GetDetail(), exception)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public SaeException(StatusCodes code, string message, Exception exception) : this((int)code, message, exception)
        {
            this.Code = (int)code;
        }

        public SaeException(int code, string message, Exception exception) : base(message, exception)
        {
            this.Code = code;
        }
    }
}
