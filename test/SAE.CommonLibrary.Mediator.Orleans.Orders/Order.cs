using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.Mediator.Orleans.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class Order
    {
        public string Id { get; set; }
        public IEnumerable<Product.Product> Products { get; set; }
    }
}
