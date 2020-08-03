using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document
{
    public abstract class Document : IDocument
    {
        public Document()
        {
            this._status = new List<IEvent>();
        }
        private IList<IEvent> _status;
        IIdentity IDocument.Identity => this.GetIdentity().ToIdentity();

        IEnumerable<IEvent> IDocument.ChangeEvents => this._status;

        int IDocument.Version { get; set; }

        void IDocument.Mutate(IEvent @event)
        {
            this.Extend(this.GetType(), @event.GetType(), @event);
        }

        protected virtual void Apply(IEvent @event)
        {
            this._status.Add(@event);

            ((IDocument)this).Mutate(@event);
        }

        protected virtual void Apply<TEvent>(object command, Action<TEvent> action = null) where TEvent : IEvent
        {
            var @event = command.To<TEvent>();
            action?.Invoke(@event);
            this.Apply(@event);
        }
        protected virtual string GetIdentity()
        {
            dynamic @dynamic = this;
            return @dynamic.Id.ToString();
        }
    }
}
