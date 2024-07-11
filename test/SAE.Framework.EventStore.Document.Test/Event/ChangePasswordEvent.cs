using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore.Document.Memory.Test.Event
{
    public class ChangePasswordEvent:IEvent
    {
        public string Password { get; set; }

    }
}
