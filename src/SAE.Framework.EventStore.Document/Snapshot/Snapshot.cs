﻿using System;
using System.Collections.Generic;
using System.Text;
using SAE.Framework.EventStore.Serialize;

namespace SAE.Framework.EventStore.Snapshot
{
    /// <summary>
    /// 快照对象
    /// </summary>
    public class Snapshot
    {
        /// <summary>
        /// 构造一个新的对象
        /// </summary>
        /// <param name="identity">标识</param>
        internal Snapshot(IIdentity identity)
        {
            this.Id = identity.ToString();
        }
        /// <summary>
        /// 构造一个新的对象
        /// </summary>
        public Snapshot()
        {

        }

        /// <summary>
        /// 构造一个新的对象
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="data">数据</param>
        /// <param name="version">版本</param>
        public Snapshot(IIdentity identity, string data, int version) : this(identity)
        {
            this.Id = identity.ToString();
            this.Data = data;
            this.Version = version;
        }
        /// <summary>
        /// 对象标识
        /// </summary>
        /// <value></value>
        public string Id { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
