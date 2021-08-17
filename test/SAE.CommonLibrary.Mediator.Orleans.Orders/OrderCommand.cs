using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class OrderCommand
    {
        public OrderCommand()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
    }
}
