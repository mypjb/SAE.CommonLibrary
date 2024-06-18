using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Abstract.Mediator
{
    /// <summary>
    /// 中介者构建接口
    /// </summary>
    public interface IMediatorBuilder
    {
        /// <summary>
        /// 处理程序描述集合
        /// </summary>
        public IEnumerable<CommandHandlerDescriptor> Descriptors { get; }
        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services { get; }
    }
    /// <summary>
    /// 中介者构建接口
    /// </summary>
    public class MediatorBuilder : IMediatorBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="descriptors">描述集</param>
        public MediatorBuilder(IServiceCollection services, List<CommandHandlerDescriptor> descriptors)
        {
            this.Services = services;
            this.Descriptors = descriptors;
        }
        /// <inheritdoc/>
        public IServiceCollection Services { get; }
        /// <inheritdoc/>
        public IEnumerable<CommandHandlerDescriptor> Descriptors { get; }
    }
}
