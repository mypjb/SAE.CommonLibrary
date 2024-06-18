using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore.Document
{
    /// <summary>
    /// <see cref="IDocument"/> 配置参数
    /// </summary>
    public class DocumentOptions
    {
        /// <summary>
        /// 配置节点
        /// </summary>
        public const string Option = "document";
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        public DocumentOptions()
        {
            this.SnapshotInterval = 5;
            this.VersionPeak = int.MaxValue;
        }
        /// <summary>
        /// 快照间隔
        /// </summary>
        public int SnapshotInterval { get; set; }
        /// <summary>
        /// 版本峰值默认<see cref="int.MaxValue"/>
        /// </summary>
        public int VersionPeak { get; }
    }
}
