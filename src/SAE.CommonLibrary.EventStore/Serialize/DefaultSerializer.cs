using System;
using SAE.CommonLibrary.Extension;
namespace SAE.CommonLibrary.EventStore.Serialize
{
    public class DefaultSerializer : ISerializer
    {
        public DefaultSerializer()
        {
        }

        public object Deserialize(string input, Type type)
        {
            return input.ToObject(type);
        }

        public string Serialize(object @object)
        {
            return @object.ToJsonString();
        }
    }
}
