using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore.Snapshot
{
    /// <summary>
    /// <see cref="ISnapshotStore"/>扩展程序
    /// </summary>
    public static class SnapshotExtension
    {
        /// <summary>
        /// 根据id和版本号从快照中查找聚合对象
        /// </summary>
        /// <param name="snapshotStore"></param>
        /// <param name="identity">标识</param>
        /// <param name="version">返回版本的快照</param>
        /// <returns>快照</returns>
        public static Snapshot Find(this ISnapshotStore snapshotStore,IIdentity identity, int version)
        {
            return snapshotStore.FindAsync(identity, version)
                                .GetAwaiter()
                                .GetResult();
        }
        /// <summary>
        /// 查找快照
        /// </summary>
        /// <param name="snapshotStore">快照存储接口</param>
        /// <param name="identity">标识</param>
        /// <returns>快照</returns>
        public static Snapshot Find(this ISnapshotStore snapshotStore,IIdentity identity)
        {
            return snapshotStore.FindAsync(identity)
                                .GetAwaiter()
                                .GetResult();
        }
        /// <summary>
        /// 保存快照
        /// </summary>
        /// <param name="snapshotStore">快照存储接口</param>
        /// <param name="snapshot">要保存的快照对象</param>
        public static void Save(this ISnapshotStore snapshotStore,Snapshot snapshot)
        {
            snapshotStore.SaveAsync(snapshot)
                         .Wait();
        }
    }
}
