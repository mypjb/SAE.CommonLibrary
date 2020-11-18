using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.MessageQueue;
using SAE.CommonLibrary.MessageQueue.Memory;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageQueueDependencyInjectionExtension
    {
        /// <summary>
        /// 添加MQ内存实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryMessageQueue(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMessageQueue, MemoryMessageQueue>();
            serviceCollection.AddNlogLogger();
            return serviceCollection;
        }
    }
}
