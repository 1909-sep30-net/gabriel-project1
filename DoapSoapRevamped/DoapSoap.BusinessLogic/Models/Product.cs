using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public SpiceLevel Spice { get; set; }
    }
}
