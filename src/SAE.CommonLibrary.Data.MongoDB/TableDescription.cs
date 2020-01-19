using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public class TableDescription<T> where T : class
    {
        public TableDescription()
        {
            var type = typeof(T);

            this.TableName = type.Name.ToLower();

            var property = type.GetProperties()
                               .Where(s => s.Name.ToLower() == "_id" || s.Name.ToLower() == "id")
                               .FirstOrDefault();

            if (property == null)
            {
                throw new ArgumentNullException(nameof(TableName), $"{type.FullName}中必须要有一个，唯一的键。默认为\"_id或\"\"id\"");
            }

            var p = Expression.Parameter(typeof(T));

            var body = Expression.Property(p, property.Name);

            var expression = Expression.Lambda(body, p);

            var @delegate= expression.Compile();

            this.IdentiyFactory = s => @delegate.DynamicInvoke(s);
        }

        public TableDescription(string tableName, Func<T, object> identiyFactory)
        {
            this.TableName = tableName;
            this.IdentiyFactory = identiyFactory;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; }
        public Func<T, object> IdentiyFactory { get; }
    }
}
