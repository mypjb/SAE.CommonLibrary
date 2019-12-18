using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document.Memory.Test.Event
{
    public class CreateEvent:IEvent
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }

        public long Version { get; set; }
    }
}
