namespace SAE.Framework.EventStore.Document.Memory.Test.Event
{
    public class SetBasicPropertyEvent:IEvent
    {
        public SetBasicPropertyEvent()
        {

        }

        public string Name { get; set; }
        public int Sex { get; set; }


    }
}
