using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class CartViewModel
    {
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Spice Level")]
        public string SpiceLevel { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
