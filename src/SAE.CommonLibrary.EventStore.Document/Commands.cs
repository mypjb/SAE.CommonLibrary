using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.EventStore.Document
{
    public class RemoveCommand<TDocument> where TDocument : IDocument
    {
        public string Id { get; set; }
    }


    public class BatchRemoveCommand<TDocument> where TDocument : IDocument
    {
        public IEnumerable<string> Ids { get; set; }
    }
    public class ListCommand
    {

    }
}
