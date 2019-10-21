using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class AddProductViewModel
    {
        [DisplayName("Selected Customer")]
        public Customer SelectedCustomer { get; set; }
        public int CustomerID { get; set; }

        [DisplayName("Selected Location")]
        public Location SelectedLocation { get; set; }
        public int LocationID { get; set; }

        /// <summary>
        /// Cart that gets updated with each call
        /// </summary>
        public IDictionary<int, int> Cart { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// The cart that gets populated and displayed
        /// </summary>
        public IDictionary<Product, int> DisplayCart { get; set; } = new Dictionary<Product, int>();


        [Required]
        public int SelectedProductID { get; set; }

        [DisplayName("Quantity")]
        [Required]
        public int SelectedQuantity { get; set; }

        public IDictionary<Product,int> Inventory { get; set; }

        public IDictionary<int,int> HiddenInventory { get; set; }
    }
}
