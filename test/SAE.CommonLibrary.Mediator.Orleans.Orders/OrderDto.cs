﻿using SAE.CommonLibrary.EventStore.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans.Orders
{
    public class Order:Document
    {
        public string Id { get; set; }
    }
}
