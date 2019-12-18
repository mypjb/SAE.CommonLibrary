﻿namespace SAE.CommonLibrary.EventStore.Document.Memory.Test.Event
{
    public class SetBasicPropertyEvent:IEvent
    {
        public SetBasicPropertyEvent()
        {

        }

        public string Name { get; set; }
        public int Sex { get; set; }

        public string Id { get; set; }

        public long Version { get; set; }
    }
}
