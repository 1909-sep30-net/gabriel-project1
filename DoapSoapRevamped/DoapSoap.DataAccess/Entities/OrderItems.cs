﻿using System;
using System.Collections.Generic;

namespace DoapSoap.DataAccess.Entities
{
    public partial class OrderItems
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
