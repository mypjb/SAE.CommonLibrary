using System;

namespace SAE.CommonLibrary.Abstract.Model
{
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
        public Paging()
        {
            this.DefaultIndex = 1;
            this.DefaultSize = 10;
            this.PageIndex = 1;
            this.PageSize = 10;
        }


        public int TotalCount
        {
            get; set;
        }
        public int TotalPages
        {
            get; set;
        }
        private int pageIndex;
        public int PageIndex
        {
            get => this.pageIndex;
            set => this.pageIndex = value <= 0 ? this.DefaultIndex : value;
        }
        private int pageSize;
        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = value <= 0 ? this.DefaultSize : value;
        }

        public int BegingRange => (this.PageIndex - 1) * this.PageSize + 1;

        public int EndRange => this.PageIndex * this.PageSize;
    }
}
