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
        /// 空的成功输出输出结果
        /// </summary>
        public static ResponseResult Success = new ResponseResult();
        /// <summary>
        /// 使用<paramref name="body"/>作为主体创建一个<seealso cref="ResponseResult{TBody}"/>对象
        /// </summary>
        /// <typeparam name="TBody">主体类型</typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ResponseResult<TBody> Create<TBody>(TBody body)
        {
            return new ResponseResult<TBody>(body);
        }
        /// <summary>
        /// 
        /// </summary>
        public ResponseResult() : this(StatusCode.Success)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        public ResponseResult(object body) : this()
        {
            this.Body = body;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public ResponseResult(StatusCode code) : this(code, string.Empty)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(StatusCode code, string message)
        {
            this.StatusCode = code;
            this.message = message;
        }
        public ResponseResult(Exception exception) : this(StatusCode.Unknown, exception)
        {

        }

        public ResponseResult(StatusCode code, Exception exception) : this(code, exception?.Message)
        {
        }

        public ResponseResult(SaeException exception) : this(StatusCode.Custom, exception)
        {

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
    /// <typeparam name="TBody"></typeparam>

    public class ResponseResult<TBody> : ResponseResult
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
        public ResponseResult(TBody body) : base(body)
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
        public new TBody Body
        {
            get => (TBody)base.Body;
            set => base.Body = value;
        }
    }
}
