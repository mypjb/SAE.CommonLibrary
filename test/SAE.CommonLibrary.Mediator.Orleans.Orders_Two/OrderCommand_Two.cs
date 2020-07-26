using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_Two
{
    public class OrderCommand_Two
    {
        public OrderCommand_Two()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
    }
}
