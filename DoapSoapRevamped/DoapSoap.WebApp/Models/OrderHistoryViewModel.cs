using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class OrderHistoryViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Customer Name")]
        [Required]
        public string CustomerName { get; set; }

        [DisplayName("Location Name")]
        [Required]
        public string LocationName { get; set; }

        [DisplayName("Time Ordered")]
        [Required]
        public DateTime Time { get; set; }

        public BusinessLogic.Models.Customer Customer { get; set; }
    }
}
