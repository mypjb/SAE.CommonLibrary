using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// 回滚
        /// </summary>
        void RollBack();
    }
}
