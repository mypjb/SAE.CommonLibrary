using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore
{
    public class EventMappingProvider
    {
        public EventMappingProvider(IEnumerable<Type> types)
        {
            this.Types = types;
        }
        public IEnumerable<Type> Types { get; }
    }
}
