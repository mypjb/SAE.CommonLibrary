using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Data
{
    public class Metadata<T> where T : class
    {
        public Metadata()
        {

        }
        public Metadata(string name) : this(name, null)
        {

        }
        public Metadata(string name, Func<T, object> identiyFactory)
        {
            var type = typeof(T);

            if (name.IsNullOrWhiteSpace())
            {
                name = type.Name.ToLower();
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
        public Func<T, object> IdentiyFactory { get; }
    }
}
