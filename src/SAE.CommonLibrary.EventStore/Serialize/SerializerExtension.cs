using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Serialize
{
    /// <summary>
    /// 事件序列化器扩展程序
    /// </summary>
    public static class SerializerExtension
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="json">反序列化对象的json形式</param>
        /// <typeparam name="T">反序列的类型</typeparam>
        public static T Deserialize<T>(this ISerializer serializer,string json)
        {
            return (T)serializer.Deserialize(json, typeof(T));
        }
    }
}
