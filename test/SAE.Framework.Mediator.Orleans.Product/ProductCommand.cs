using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Mediator.Orleans.Product
{
    public class ProductCommand
    {
        public ProductCommand()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
    }
    
}
