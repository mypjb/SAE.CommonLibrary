using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.EventStore.Document
{
    public class Command
    {
        public class Find<TDot>
        {
            public string Id { get; set; }
        }
        public class List<TDot>
        {

        }
        public class Delete<T>
        {
            public string Id { get; set; }
        }
        public class BatchDelete<T>
        {
            public IEnumerable<string> Ids { get; set; }
        }
    }
   
}
