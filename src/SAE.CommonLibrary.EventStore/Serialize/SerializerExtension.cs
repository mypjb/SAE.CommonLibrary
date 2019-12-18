using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Serialize
{
    public static class SerializerExtension
    {
        public static T Deserialize<T>(this ISerializer serializer,string json)
        {
            return (T)serializer.Deserialize(json, typeof(T));
        }
    }
}
