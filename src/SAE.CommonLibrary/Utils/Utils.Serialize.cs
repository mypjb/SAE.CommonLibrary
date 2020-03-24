using Newtonsoft.Json;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SAE.CommonLibrary
{
    public partial class Utils
    {
        /// <summary>
        /// 序列化
        /// </summary>
        public class Serialize
        {
            /// <summary>
            /// Json序列化
            /// </summary>
            /// <param name="object"></param>
            /// <returns></returns>
            public static string Json(object @object)
            {
                if (@object.IsNull())
                {
                    return string.Empty;
                }
                return JsonConvert.SerializeObject(@object, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
            }
       
            /// <summary>
            /// 将<paramref name="object"/>序列化为<seealso cref="XDocument"/>
            /// </summary>
            /// <param name="object"></param>
            /// <returns></returns>
            public static XDocument Xml(object @object)
            {
                var serializer = new XmlSerializer(@object.GetType());
                XDocument document = new XDocument();
                serializer.Serialize(document.CreateWriter(), @object);
                return document;
            }

            /// <summary>
            /// 将<paramref name="json"/>序列化为<seealso cref="XDocument"/>
            /// </summary>
            /// <param name="json"></param>
            /// <returns></returns>
            public static XDocument Xml(string json)
            {
                return JsonConvert.DeserializeXNode(json) ?? null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public class Deserialize
        {
            /// <summary>
            /// Json反序列化
            /// </summary>
            /// <param name="json"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static object Json(string json, Type type)
            {
                if (json.IsNullOrWhiteSpace()) return null;
                return JsonConvert.DeserializeObject(json, type);
            }

            /// <summary>
            /// 将<paramref name="document"/>,返序列化为<paramref name="type"/>对象
            /// </summary>
            /// <param name="document"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static object Xml(XDocument document, Type type)
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(document.CreateReader());
            }
        }

    }
}
