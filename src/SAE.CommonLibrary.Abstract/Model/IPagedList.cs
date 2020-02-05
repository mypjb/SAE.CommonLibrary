using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAE.CommonLibrary.Abstract.Model
{
    public interface IPagedList : IEnumerable, IPaging
    {

    }
    public interface IPagedList<T> : IEnumerable<T>, IPagedList
    {

    }
}
