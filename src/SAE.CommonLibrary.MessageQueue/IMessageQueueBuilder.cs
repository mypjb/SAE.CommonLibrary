using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.MessageQueue
{
    /// <summary>
    /// <see cref="IMessageQueue"/>构建者
    /// </summary>
    public interface IMessageQueueBuilder
    {
        /// <summary>
        /// 服务接口
        /// </summary>
        /// <value></value>
        IServiceCollection Services { get; }
    }
    /// <summary>
    /// <see cref="IMessageQueueBuilder"/>构建者实现
    /// </summary>
    public class MessageQueueBuilder : IMessageQueueBuilder
    {
        public MessageQueueBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        public IServiceCollection Services { get; }
    }
}