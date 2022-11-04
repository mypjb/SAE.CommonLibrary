using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region 序列化
        /// <summary>
        /// 将对象转换成Json字符串
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ToJsonString(this object @object)
        {
            return Utils.Serialize.Json(@object);
        }
        /// <summary>
        /// 将<paramref name="json"/>转换成<paramref name="type"/>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string json, Type type)
        {
            return Utils.Deserialize.Json(json, type);
        }
        /// <summary>
        /// 将<paramref name="json"/>转换成<typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return (T)json.ToObject(typeof(T));
        }
        /// <summary>
        /// 将<paramref name="json"/>序列化为<seealso cref="XDocument"/>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>

        public static XDocument ToXml(this string json)
        {
            return Utils.Serialize.Xml(json);
        }
        /// <summary>
        /// 将<paramref name="object"/>序列化为<seealso cref="XDocument"/>
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>

        public static XDocument ToXml(this object @object)
        {
            return Utils.Serialize.Xml(@object);
        }

        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <param name="document">xml对象</param>
        /// <param name="type">反序列类型</param>
        public static object ToObject(this XmlDocument document, Type type)
        {
            using (var nodeReader = new XmlNodeReader(document))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader).ToObject(type);
            }
        }
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <param name="document">xml对象</param>
        /// <param name="type">反序列类型</param>
        public static object ToObject(this XDocument document, Type type)
        {
            return Utils.Deserialize.Xml(document, type);
        }
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <typeparam name="T">反序列类型</typeparam>
        public static T ToObject<T>(this XmlDocument document) where T : class
        {
            return document.ToObject(typeof(T)) as T;
        }
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <typeparam name="T">反序列类型</typeparam>
        public static T ToObject<T>(this XDocument document) where T : class
        {
            return document.ToObject(typeof(T)) as T;
        }
        #endregion
    }


}
