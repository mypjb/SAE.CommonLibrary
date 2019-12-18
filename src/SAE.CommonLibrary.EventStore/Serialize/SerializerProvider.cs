using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Serialize
{
    /// <summary>
    /// 序列化器提供程序
    /// </summary>
    public class SerializerProvider
    {
        static SerializerProvider()
        {
            Current =new DefaultSerializer();    
        }

        /// <summary>
        /// 当前序列化器
        /// </summary>
        public static ISerializer Current
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 设置序列化器
        /// </summary>
        /// <param name="serializer"></param>
        public static void SetSerializer(ISerializer serializer)
        {
            Current = serializer;
        }
    }
}
