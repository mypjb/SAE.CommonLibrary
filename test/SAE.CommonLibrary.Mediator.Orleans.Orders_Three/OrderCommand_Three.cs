using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders_Three
{
    public class OrderCommand_Three
    {
        public OrderCommand_Three()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
    }
}
