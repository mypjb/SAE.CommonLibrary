using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_One
{
    public class OrderCommand_One
    {
        public OrderCommand_One()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
    }
}
