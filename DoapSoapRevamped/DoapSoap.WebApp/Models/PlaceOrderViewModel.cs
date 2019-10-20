using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class PlaceOrderViewModel
    {
        // list of locations
        [DisplayName("Locations")]
        [Required]
        public IEnumerable<Location> locations { get; set; } = new List<Location>();

        // list of customers
        [DisplayName("Customers")]
        [Required]
        public IEnumerable<Customer> customers { get; set; } = new List<Customer>();

        public Location selectedLocation { get; set; }
        public Customer selectedCustomer { get; set; }

        public bool selectedOptions { get; set; }

    }
}
