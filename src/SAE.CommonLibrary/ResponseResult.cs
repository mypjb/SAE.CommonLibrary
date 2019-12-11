using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 标准输出
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseResult():this(StatusCode.Success)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        public ResponseResult(object body):this()
        {
            this.Body = body;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public ResponseResult(StatusCode code):this(code,string.Empty)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(StatusCode code,string message)
        {
            this.StatusCode = code;
            this.message = message;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode StatusCode { get; set; }
        private string message;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get
            {
                return this.message.IsNullOrWhiteSpace() ? this.StatusCode.ToString() : this.message;
            }
            set
            {
                this.message = value;
            }
        }
        /// <summary>
        /// 主体
        /// </summary>
        public object Body { get; set; }
    }
    /// <summary>
    /// <seealso cref="ResponseResult"/>的泛型实现
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class ResponseResult<T> : ResponseResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseResult()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        public ResponseResult(T body) : base(body)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public ResponseResult(StatusCode code) : base(code)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(StatusCode code, string message) : base(code, message)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public new T Body
        {
            get => (T)base.Body;
            set => base.Body = value;
        }
    }
}
