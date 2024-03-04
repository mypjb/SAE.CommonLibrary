using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SAE.CommonLibrary.Extension;

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
            /// 创建静态实例
            /// </summary>
            static Serialize()
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
            }
            /// <summary>
            /// Json序列化
            /// </summary>
            /// <param name="object">序列化对象</param>
            /// <returns>json字符串</returns>
            public static string Json(object @object)
            {
                if (@object.IsNull())
                {
                    return string.Empty;
                }

                if (@object.GetType() == Utils.Deserialize.StringType) return @object.ToString();

                return JsonConvert.SerializeObject(@object);
            }

            /// <summary>
            /// 将<paramref name="object"/>序列化为<seealso cref="XDocument"/>
            /// </summary>
            /// <param name="object">序列化对象</param>
            /// <returns>xml文档</returns>
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
            /// <param name="json">json字符串</param>
            /// <returns>xml文档</returns>
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
            /// string类型
            /// </summary>
            internal static readonly Type StringType = typeof(string);
            /// <summary>
            /// 创建静态对象
            /// </summary>
            static Deserialize()
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
            }


            /// <summary>
            /// Json反序列化
            /// </summary>
            /// <param name="json">json字符串</param>
            /// <param name="type">反序列化的类型</param>
            /// <returns>反序列化对象</returns>
            public static object Json(string json, Type type)
            {
                if (json.IsNullOrWhiteSpace()) return null;

                if (type == StringType) return json;

                return JsonConvert.DeserializeObject(json, type);
            }
            /// <summary>
            /// 使用<paramref name="json"/> 的值填充现有对象实例。
            /// </summary>
            /// <param name="json">json字符串</param>
            /// <param name="object">要填充的对象</param>
            public static void PopulateObject(string json, object @object)
            {
                if (json.IsNullOrWhiteSpace() || @object == null) return;

                if (@object.GetType() == StringType) return;

                JsonConvert.PopulateObject(json, @object);
            }

            /// <summary>
            /// 将<paramref name="document"/>,返序列化为<paramref name="type"/>对象
            /// </summary>
            /// <param name="document"></param>
            /// <param name="type"></param>
            /// <returns>反序列化对象</returns>
            public static object Xml(XDocument document, Type type)
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(document.CreateReader());
            }
        }

    }
}
