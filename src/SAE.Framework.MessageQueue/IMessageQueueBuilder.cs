using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.MessageQueue
{
    /// <summary>
    /// <see cref="IMessageQueue"/>构建者
    /// </summary>
    public interface IMessageQueueBuilder
    {
        /// <summary>
        /// 服务接口
        /// </summary>
        IServiceCollection Services { get; }
    }
    /// <summary>
    /// <see cref="IMessageQueueBuilder"/>构建者实现
    /// </summary>
    public class MessageQueueBuilder : IMessageQueueBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services">服务集合</param>
        public MessageQueueBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        /// <inheritdoc/>
        public IServiceCollection Services { get; }
    }
}