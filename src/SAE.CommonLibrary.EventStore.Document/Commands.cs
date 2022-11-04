using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 查找
        /// </summary>
        /// <typeparam name="TDot"></typeparam>
        public class Find<TDot>
        {
            public string Id { get; set; }
        }
        /// <summary>
        /// 列出列表
        /// </summary>
        /// <typeparam name="TDot"></typeparam>
        public class List<TDot>
        {

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Delete<T>
        {
            public string Id { get; set; }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BatchDelete<T>
        {
            public IEnumerable<string> Ids { get; set; }
        }
    }
   
}
