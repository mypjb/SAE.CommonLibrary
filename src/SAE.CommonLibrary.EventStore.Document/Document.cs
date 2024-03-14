using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// <see cref="IDocument"/> 抽象实现
    /// </summary>
    public abstract class Document : IDocument
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Document()
        {
            this._status = new List<IEvent>();
        }
        /// <summary>
        /// 内部事件集合
        /// </summary>
        private readonly IList<IEvent> _status;

        IIdentity IDocument.Identity => this.GetIdentity().ToIdentity();


        IEnumerable<IEvent> IDocument.ChangeEvents => this._status;

        int IDocument.Version { get; set; }

        void IDocument.Mutate(IEvent @event)
        {
            this.To(this.GetType(), @event.GetType(), @event);
        }
        /// <summary>
        /// 将事件应用到当前对象
        /// </summary>
        protected virtual void Apply(IEvent @event)
        {
            this._status.Add(@event);

            ((IDocument)this).Mutate(@event);
        }
        /// <summary>
        /// <para>将<paramref name="command"/>应用到当前对象。</para>
        /// <para>该函数先将<paramref name="command"/>转换成<typeparamref name="TEvent"/>，再以此调用<paramref name="action"/>和<see cref="Apply(TEvent)"/></para>
        /// </summary>
        /// <param name="command">待转换的事件对象</param>
        /// <param name="action">转化完成后，执行的回调</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        protected virtual void Apply<TEvent>(object command, Action<TEvent> action = null) where TEvent : IEvent
        {
            var @event = command.To<TEvent>();
            action?.Invoke(@event);
            this.Apply(@event);
        }
        /// <summary>
        /// 返回标识列
        /// </summary>
        protected virtual string GetIdentity()
        {
            dynamic @dynamic = this;
            return @dynamic.Id.ToString();
            // return ((IDocument)this).Identity.ToString();
        }
    }
}
