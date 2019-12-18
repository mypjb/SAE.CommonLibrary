using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.MessageQueue
{
    /// <summary>
    /// 名称工具类
    /// </summary>
    internal class Utils
    {

        /// <summary>
        /// 获得队列名称
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public static string Get<TMessage>()
        {
            var type = typeof(TMessage);
            var identity= typeof(TMessage).FullName.ToMd5(true);
            var logging = ServiceFacade.GetService<ILogging<Utils>>();
            logging.Debug($"简化标识'{type.Name}' => '{identity}'");
            return identity;
        }

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
