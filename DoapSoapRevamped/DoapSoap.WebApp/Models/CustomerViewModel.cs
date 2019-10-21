using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class CustomerViewModel
    {
        private string _phone;

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        public string Phone 
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        [DisplayName("Phone Number")]
        public string DisplayPhone
        {
            get
            {
                return String.Format("({0}) {1}-{2}", _phone.Substring(0, 3), _phone.Substring(3, 3), _phone.Substring(6, 4));
            }
        }

        [DisplayName("Search Name")]
        public string SearchName { get; set; }
    }
}
