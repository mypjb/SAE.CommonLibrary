using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.MessageQueue
{
    /// <summary>
    /// 名称工具类
    /// </summary>
    internal class Utils
    {

        /// <summary>
        /// 获得队列名称
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <returns>队列名称</returns>
        public static string Get<TMessage>()
        {
            var type = typeof(TMessage);
            var identity = type.GetIdentity();
            var logging = ServiceFacade.GetService<ILogging<Utils>>();
            logging.Debug($"简化标识'{type.Name}' => '{identity}'");
            return identity;
        }
        /// <summary>
        /// 获得队列名称
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">消息</param>
        /// <returns>队列名称</returns>
        public static string Get<TMessage>(TMessage message)
        {
            if (message is IIdentity)
            {
                return ((IIdentity)message).Identity;
            }
            return Get<TMessage>();
        }
    }
}
