using System;
using SAE.CommonLibrary.Extension;
namespace SAE.CommonLibrary.EventStore.Serialize
{
    /// <summary>
    /// <see cref="ISerializer"/> 默认实现
    /// </summary>
    /// <inheritdoc/>
    public class DefaultSerializer : ISerializer
    {

        /// <summary>
        /// 
        /// </summary>
        public DefaultSerializer()
        {
        }
        
        public object Deserialize(string input, Type type)
        {
            return input.ToObject(type);
        }

        public void Deserialize(string input, object @object)
        {
            @object.JsonExtend(input);
        }

        public string Serialize(object @object)
        {
            return @object.ToJsonString();
        }
    }
}
