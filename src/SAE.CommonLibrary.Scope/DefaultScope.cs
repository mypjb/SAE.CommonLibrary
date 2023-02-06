using System;
using System.Net;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    internal class DefaultScope : IScope
    {
        /// <summary>
        /// 以前的区域
        /// </summary>
        private readonly IScope _previous;
        /// <summary>
        /// 区域释放的事件
        /// </summary>
        public event Func<IScope, Task> OnDispose;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">scope name</param>
        public DefaultScope(string name)
        {
            this.Name=name;
            this._previous=this;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">新的区域名称</param>
        /// <param name="previous">释放时重置为此值 <seealso cref="Dispose()"/></param>
        public DefaultScope(string name, IScope previous)
        {
            this.Name=name;
            this._previous=previous;
        }

        public string Name
        {
            get;
        }

        public void Dispose()
        {
            this.OnDispose?.Invoke(this._previous);
        }
    }
}