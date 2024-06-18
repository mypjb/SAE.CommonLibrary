using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SAE.Framework.Extension;

namespace SAE.Framework.Data
{
    /// <summary>
    /// 元数据
    /// </summary>
    /// <remarks>
    /// 标识对象在<see cref="IStorage"/>中如何进行存储的
    /// </remarks>
    /// <typeparam name="T">元数据对应的类型</typeparam>
    public class Metadata<T> where T : class
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Metadata() : this(null, null)
        {

        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">元数据的名称</param>
        public Metadata(string name) : this(name, null)
        {

        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">元数据的名称</param>
        /// <param name="identiyFactory">标识工厂，可以通过它获取对象的标识</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Metadata(string name, Func<T, object> identiyFactory)
        {
            var type = typeof(T);

            if (name.IsNullOrWhiteSpace())
            {
                name = type.Name.ToLower();
            }
            else
            {
                name = name.ToLower();
            }

            if (identiyFactory == null)
            {
                var property = type.GetProperties()
                               .Where(s => s.Name.ToLower() == "_id" || s.Name.ToLower() == "id")
                               .FirstOrDefault();

                if (property == null)
                {
                    throw new ArgumentNullException(nameof(Name), $"{type.FullName}中必须要有一个，唯一的键。默认为\"_id或\"\"id\"");
                }

                var p = Expression.Parameter(typeof(T));

                var body = Expression.Property(p, property.Name);

                var expression = Expression.Lambda(body, p);

                var @delegate = expression.Compile();

                identiyFactory = s => @delegate.DynamicInvoke(s);
            }

            this.Name = name;
            this.IdentiyFactory = identiyFactory;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 标识工厂
        /// </summary>
        /// <remarks>
        /// 如果没有设置，默认使用对象的<c>id</c>或<c>_id</c>作为标识
        /// </remarks>
        public Func<T, object> IdentiyFactory { get; }
    }
}
