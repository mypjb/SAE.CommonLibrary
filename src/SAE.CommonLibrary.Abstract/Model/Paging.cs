using System;

namespace SAE.CommonLibrary.Abstract.Model
{
    /// <summary>
    /// 分页实现
    /// </summary>
    public class Paging : IPaging
    {
        /// <summary>
        /// 默认的页数索引
        /// </summary>
        protected int DefaultIndex { get; }
        /// <summary>
        /// 默认的页面大小
        /// </summary>
        protected int DefaultSize { get; }
        /// <summary>
        /// ctor
        /// </summary>
        public Paging()
        {
            this.DefaultIndex = 1;
            this.DefaultSize = 10;
            this.PageIndex = 1;
            this.PageSize = 10;
        }


        int IPaging.TotalCount
        {
            get; set;
        }
        int IPaging.TotalPages
        {
            get; set;
        }
        private int pageIndex;
        /// <inheritdoc/>
        public int PageIndex
        {
            get => this.pageIndex;
            set => this.pageIndex = value <= 0 ? this.DefaultIndex : value;
        }
        private int pageSize;
        /// <inheritdoc/>
        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = value <= 0 ? this.DefaultSize : value;
        }

        int IPaging.BeginRange => (this.PageIndex - 1) * this.PageSize + 1;

        int IPaging.EndRange => this.PageIndex * this.PageSize;
    }
}
