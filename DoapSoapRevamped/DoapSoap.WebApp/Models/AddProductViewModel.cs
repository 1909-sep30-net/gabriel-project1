using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class AddProductViewModel
    {
        [DisplayName("Selected Customer")]
        public Customer SelectedCustomer { get; set; }

        [DisplayName("Selected Location")]
        public Location SelectedLocation { get; set; }

        public IDictionary<Product, int> Cart { get; set; } = new Dictionary<Product, int>();

        public KeyValuePair<Product,int> SelectedProduct { get; set; }

        public IDictionary<Product,int> Inventory { get; set; }
    }
}
