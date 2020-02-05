using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SAE.CommonLibrary.Abstract.Model
{
    /// <summary>
    /// 分页通用类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    [Serializable]
    [JsonObject]
    public class PagedList<T> : IEnumerable<T>, IPagedList<T>
    {
        [JsonProperty(PropertyName = "items")]
        private readonly List<T> _score;

        /// <summary>
        /// 
        /// </summary>
        public PagedList()
        {
            this._score = new List<T>();
        }
        /// <summary>
        /// 数据源为IQueryable的范型
        /// </summary>
        /// <param name="querySource">数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        public PagedList(IQueryable<T> querySource, int pageIndex, int pageSize) : this()
        {
            if (querySource != null) //判断传过来的实体集是否为空
            {
                int total = querySource.Count();
                this.TotalCount = total;
                this.TotalPages = (int)Math.Ceiling(total / 1.0 / pageSize);

                this.PageSize = pageSize;
                if (pageIndex > this.TotalPages)
                {
                    pageIndex = this.TotalPages;
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                this.PageIndex = pageIndex;
                this._score.AddRange(querySource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()); //Skip是跳到第几页，Take返回多少条
            }
        }
        /// <summary>
        /// 数据源为IEnumerable的范型
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        /// <param name="totalCount">总记录数</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount) : this()
        {
            if (source != null) //判断传过来的实体集是否为空
            {
                TotalCount = totalCount;
                TotalPages = TotalCount / pageSize;

                if (TotalCount % pageSize > 0)
                    TotalPages++;

                this.PageSize = pageSize;
                if (pageIndex > this.TotalPages)
                {
                    pageIndex = this.TotalPages;
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                this.PageIndex = pageIndex;
                this._score.AddRange(source);
            }
        }

        /// <summary>
        /// 数据源为IQueryable的范型
        /// </summary>
        /// <param name="querySource">数据源</param>
        /// <param name="paging">分页对象</param>
        public PagedList(IQueryable<T> querySource, IPaging paging) :
                         this(querySource, paging.PageIndex, paging.PageSize)
        {

        }
        /// <summary>
        /// 数据源为IEnumerable的范型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paging">分页对象</param>
        public PagedList(IEnumerable<T> source, IPaging paging) : this(source, paging.PageIndex, paging.PageSize, paging.TotalCount)
        {

        }

        [JsonProperty(PropertyName = "totalPages")]
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        [JsonProperty(PropertyName = "totalCount")]
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        [JsonProperty(PropertyName = "pageIndex")]
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        [JsonProperty(PropertyName = "pageSize")]
        /// <summary>
        /// 每页显示多少条记录
        /// </summary>
        public int PageSize { get; set; }

        int IPaging.BegingRange { get; }
        int IPaging.EndRange { get; }

        /// <summary>
        /// 返回循环访问<seealso cref="IPagedList{T}"/>的枚举数
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._score.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static class PagedList
    {
        public static IPagedList<T> Build<T>(IEnumerable<T> source, IPaging paging)
            => new PagedList<T>(source, paging);

        public static IPagedList<T> Build<T>(IQueryable<T> source, IPaging paging)
            => new PagedList<T>(source, paging);
    }
}
