using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ISet<Order> OrderHistory = new HashSet<Order>();
        public IDictionary<Product, int> Inventory = new Dictionary<Product, int>();

        public void RemoveFromInventory(Product product, int quantity)
        {
            if (Inventory[product] - quantity < 0)
            {
                throw new Exception("Quantity cannot reduce inventory past 0.");
            } 
            else
            {
                Inventory[product] -= quantity;
            }
        }
    }
}
