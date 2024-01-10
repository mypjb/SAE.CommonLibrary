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
            /// <summary>
            /// 标识
            /// </summary>
            /// <value></value>
            public string Id { get; set; }
        }
        /// <summary>
        /// 查找多个
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        public class Finds<TDto>
        {
            /// <summary>
            /// 标识集合
            /// </summary>
            public IEnumerable<string> Ids { get; set; }
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
            /// <summary>
            /// 标识
            /// </summary>
            /// <value></value>
            public string Id { get; set; }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BatchDelete<T>
        {
            /// <summary>
            /// 标识集合
            /// </summary>
            /// <value></value>
            public IEnumerable<string> Ids { get; set; }
        }
    }
}
