using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document
{
    public class DocumentOptions
    {
        public const string Option = "document";
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
        /// 版本峰值默认 <seealso cref="int.MaxValue"/>
        /// </summary>
        public int VersionPeak { get; }
    }
}
