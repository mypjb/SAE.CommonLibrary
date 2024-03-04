using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAE.CommonLibrary.Abstract.Model
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedList : IEnumerable, IPaging
    {

    }
    /// <summary>
    /// 分页接口
    /// </summary>
    /// <typeparam name="T">分页类型</typeparam>
    public interface IPagedList<T> : IEnumerable<T>, IPagedList
    {

    }
}
