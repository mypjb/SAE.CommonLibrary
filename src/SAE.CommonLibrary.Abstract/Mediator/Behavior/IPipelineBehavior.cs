using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 行为管道标记接口，只用于标记。应当改用<see cref="IPipelineBehavior{TCommand, TResponse}"/>、<see cref="IPipelineBehavior{TCommand}"/>进行实现
    /// </summary>
    [Obsolete("行为管道标记接口")]
    public interface IPipelineBehavior
    {

    }
    /// <summary>
    /// 中介者行为管道接口
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    /// <typeparam name="TResponse">响应</typeparam>
    public interface IPipelineBehavior<TCommand, TResponse>: IPipelineBehavior where TCommand : class
    {
        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="next">下一个管道委托</param>
        /// <returns>管道响应</returns>
        public Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next);
    }
    /// <summary>
    /// 中介者行为管道接口
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    public interface IPipelineBehavior<TCommand>: IPipelineBehavior where TCommand : class
    {
        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="next">下一个管道委托</param>
        public Task ExecutionAsync(TCommand command, Func<Task> next);
    }
}
