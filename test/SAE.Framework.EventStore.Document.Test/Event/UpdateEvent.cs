using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore.Document.Memory.Test.Event
{
    public class UpdateEvent : IEvent
    {
        public string Name { get; set; }
        public int Sex { get; set; }
    }
}
