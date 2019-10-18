using System;
using System.Collections.Generic;

namespace DoapSoap.DataAccess.Entities
{
    public partial class Products
    {
        public Products()
        {
            InventoryItems = new HashSet<InventoryItems>();
            OrderItems = new HashSet<OrderItems>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public int SpiceId { get; set; }
        public decimal Price { get; set; }

        public virtual SpiceLevels Spice { get; set; }
        public virtual ICollection<InventoryItems> InventoryItems { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
