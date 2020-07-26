using SAE.CommonLibrary.EventStore.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_One
{
    public class Order_One : Document
    {
        public string Id { get; set; }
    }
}
