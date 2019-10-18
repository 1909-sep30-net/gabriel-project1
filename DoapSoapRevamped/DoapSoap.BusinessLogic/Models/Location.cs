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
        public Dictionary<Product, int> Inventory = new Dictionary<Product, int>();
    }
}
