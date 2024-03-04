namespace SAE.CommonLibrary.Abstract.Model
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// 记录数
        /// </summary>
        int TotalCount { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        int TotalPages { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get;}
        /// <summary>
        /// 起始范围
        /// </summary>
        int BeginRange { get; }
        /// <summary>
        /// 结束范围
        /// </summary>
        int EndRange { get;  }
    }
}
