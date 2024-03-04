using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract
{
    /// <summary>
    /// 排序接口
    /// </summary>
    public interface IOrdered
    {
        /// <summary>
        /// 排序依据
        /// </summary>
        public int Order { get; }
    }
}
