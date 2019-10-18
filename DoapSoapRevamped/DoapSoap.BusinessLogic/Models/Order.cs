using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Order
    {
        /// <summary>
        /// Order's unique identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The time this order was placed
        /// </summary>
        public DateTime TimePlaced { get; set; }

        /// <summary>
        /// The customer that placed this order
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// This location this order was placed for
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Dictionary that maps the quantity to products in this order
        /// </summary>
        public Dictionary<Product, int> ProductList = new Dictionary<Product, int>();

        /// <summary>
        /// Return total value of the order
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            decimal sum = 0;
            var pl = ProductList.ToList();
            foreach (KeyValuePair<Product,int> item in pl)
            {
                sum += item.Value * item.Key.Price;
            }
            return sum;
        }
    }
}
