using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 行为管道包装器
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    /// <typeparam name="TResponse">响应</typeparam>
    internal class PipelineBehaviorWrapper<TCommand, TResponse> where TCommand : class
    {
        /// <summary>
        /// 管道行为装饰器
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="proxy">代理</param>
        /// <param name="next">下个管道委托</param>
        public PipelineBehaviorWrapper(TCommand command,
                                       IPipelineBehavior<TCommand, TResponse> proxy,
                                       Func<Task<TResponse>> next)
        {
            this.Command = command;
            this.Proxy = proxy;
            this.Next = next;
        }
        /// <summary>
        /// 代理
        /// </summary>
        public IPipelineBehavior<TCommand, TResponse> Proxy { get; }
        /// <summary>
        /// 命令
        /// </summary>
        public TCommand Command { get; }
        /// <summary>
        /// 下个管道委托
        /// </summary>
        public Func<Task<TResponse>> Next { get; }
        /// <summary>
        /// 构建委托
        /// </summary>
        /// <returns>返回响应委托</returns>
        public Func<Task<TResponse>> Build()
        {
            return () => this.Proxy.ExecutionAsync(this.Command,this.Next);
        }
    }
    /// <summary>
    /// 管道行为装饰器
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    internal class PipelineBehaviorWrapper<TCommand> where TCommand : class
    {
        /// <summary>
        /// 管道行为装饰器
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="proxy">代理</param>
        /// <param name="next">下个管道委托</param>
        public PipelineBehaviorWrapper(TCommand command,
                                      IPipelineBehavior<TCommand> proxy,
                                      Func<Task> next)
        {
            this.Command = command;
            this.Proxy = proxy;
            this.Next = next;
        }
        /// <summary>
        /// 代理
        /// </summary>
        public IPipelineBehavior<TCommand> Proxy { get; }
        /// <summary>
        /// 命令
        /// </summary>
        public TCommand Command { get; }
        /// <summary>
        /// 下个管道委托
        /// </summary>
        public Func<Task> Next { get; }
        /// <summary>
        /// 构建委托
        /// </summary>
        /// <returns>返回委托</returns>
        public Func<Task> Build()
        {
            return () => this.Proxy.ExecutionAsync(this.Command, this.Next);
        }
    }
}
