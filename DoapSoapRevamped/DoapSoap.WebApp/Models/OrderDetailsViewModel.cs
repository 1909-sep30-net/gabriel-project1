using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class OrderDetailsViewModel
    {
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }

        [DisplayName("Quantity")]
        [Required]
        public int Quantity { get; set; }

        [DisplayName("Spice Level")]
        [Required]
        public string Spice { get; set; }

        [DisplayName("Price")]
        [Required]
        public decimal Price { get; set; }
    }
}
