using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 命令处理描述符
    /// </summary>
    public class CommandHandlerDescriptor
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="commandHandlerType">处理类型</param>
        /// <param name="types">命令和响应类型（如果有）</param>
        public CommandHandlerDescriptor(Type commandHandlerType,
                                        params Type[] types)
        {
            this.CommandHandlerType = commandHandlerType;
            this.CommandType = types[0];
            this.ResponseType = types.Length == 2 ? types[1] : null;
        }
        /// <summary>
        /// 命令处理类型
        /// </summary>
        public Type CommandHandlerType { get; }
        /// <summary>
        /// 命令类型
        /// </summary>
        public Type CommandType { get; }
        /// <summary>
        /// 响应类型
        /// </summary>
        public Type ResponseType { get; }
    }
}
