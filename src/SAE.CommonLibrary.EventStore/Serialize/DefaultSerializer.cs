using System;
using SAE.CommonLibrary.Extension;
namespace SAE.CommonLibrary.EventStore.Serialize
{
    /// <summary>
    /// <see cref="ISerializer"/> 默认实现
    /// </summary>
    public class DefaultSerializer : ISerializer
    {

        /// <summary>
        /// ctor
        /// </summary>
        public DefaultSerializer()
        {
        }
        /// <inheritdoc/>
        public object Deserialize(string input, Type type)
        {
            return input.ToObject(type);
        }
        /// <inheritdoc/>
        public void Deserialize(string input, object @object)
        {
            @object.JsonExtend(input);
        }
        /// <inheritdoc/>
        public string Serialize(object @object)
        {
            return @object.ToJsonString();
        }
    }
}
