using SAE.CommonLibrary.EventStore.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Snapshot
{
    /// <summary>
    /// 快照对象
    /// </summary>
    public class Snapshot
    {
        internal Snapshot(IIdentity identity)
        {
            this.Id = identity.ToString();
        }
        public Snapshot()
        {

        }

        public Snapshot(IIdentity identity, object @object, int version) : this(identity)
        {
            this.Id = identity.ToString();
            this.Data = SerializerProvider.Current.Serialize(@object);
            this.Type = @object.GetType().ToString();
            this.Version = version;
        }

        public string Id { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 快照类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
