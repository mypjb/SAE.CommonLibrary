using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// error output
    /// </summary>
    public class ErrorOutput
    {
        [Obsolete("Use other constructs instead", false)]
        public ErrorOutput()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public ErrorOutput(StatusCodes code) : this(code, code.GetDetail())
        {
        }

        public ErrorOutput(int code, string message)
        {
            Assert.Build(message).NotNullOrWhiteSpace();

            this.StatusCode = code;
            this.Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ErrorOutput(StatusCodes code, string message) : this((int)code, message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ErrorOutput(Exception exception) : this(StatusCodes.Unknown, exception)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ErrorOutput(SAEException exception) : this(exception.Code, exception)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="exception"></param>
        public ErrorOutput(StatusCodes code, Exception exception) : this(code, exception?.Message)
        {
        }

        public ErrorOutput(int code, Exception exception) : this(code, exception?.Message)
        {
        }

        /// <summary>
        /// error status code 
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// error detaild
        /// </summary>
        public string Message
        {
            get; set;
        }
    }
}
