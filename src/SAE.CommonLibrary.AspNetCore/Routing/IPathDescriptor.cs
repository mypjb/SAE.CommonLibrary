using System;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Routing
{
    /// <summary>
    /// 路径描述符
    /// </summary>
    public interface IPathDescriptor
    {
        /// <summary>
        /// 路径索引，
        /// 索引总是从1开始
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 请求谓词
        /// </summary>
        /// <value></value>
        public string Method { get; }
        /// <summary>
        /// 具体路径
        /// </summary>
        /// <value></value>
        public string Path { get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        /// <value></value>
        public string Name { get; }
        /// <summary>
        /// 所属组
        /// </summary>
        /// <value></value>
        public string Group { get; }
    }
    /// <summary>
    /// <see cref="IPathDescriptor"/>实现
    /// </summary>
    /// <inheritdoc/>
    public class PathDescriptor : IPathDescriptor
    {
        /// <summary>
        /// 创建一个新对象,你不应该手动调用该函数，如果想创建一个新的<see cref="IPathDescriptor"/>,请使用同名重载。
        /// </summary>
        public PathDescriptor()
        {

        }
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="method">请求谓词</param>
        /// <param name="path">路径</param>
        /// <param name="group">组</param>
        public PathDescriptor(string name, string method, string path, string group)
        {
            this.Name = name?.ToLower().Trim();
            this.Method = method?.ToLower().Trim();
            this.Path = path?.ToLower().Trim();
            this.Group = group?.ToLower().Trim();
            if (this.Name.IsNullOrWhiteSpace())
            {
                this.Name=this.Path;
            }
        }
        /// <inheritdoc/>
        public string Method { get; set; }
        /// <inheritdoc/>
        public string Path { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Group { get; set; }
        /// <inheritdoc/>
        public int Index { get; set; }
    }
}
