using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore
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
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
    }
}
