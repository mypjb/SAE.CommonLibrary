using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Product
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Price = (decimal)new Random().NextDouble();
        }
        public string Id { get; set; }
        public string OrderId { get; set; }
        public decimal Price { get; set; }
    }
}
