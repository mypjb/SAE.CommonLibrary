using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Serialize
{
    /// <summary>
    /// 序列化器用于提供事件的序列化
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 将事件集合转化成字符串
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        string Serialize(object @object);
        /// <summary>
        /// 将字符串反序列化为事件集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Deserialize(string input,Type type);
        /// <summary>
        /// 使用字符串的值填充现有对象实例。
        /// </summary>
        /// <param name="input">事件序列化后的字符</param>
        /// <param name="object">要填充的实例</param>
        void Deserialize(string input,object @object);
    }
}
