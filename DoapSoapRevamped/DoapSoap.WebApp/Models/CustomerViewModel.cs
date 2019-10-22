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
        [MaxLength(50)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        //[StringLength(10, ErrorMessage = "The value must be 10 numbers. ")]
        [Phone(ErrorMessage ="Must be phone number format.")]
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
                if (_phone.Length == 10)
                {
                    return String.Format("({0}) {1}-{2}", _phone.Substring(0, 3), _phone.Substring(3, 3), _phone.Substring(6, 4));
                }
                else
                {
                    return _phone;
                }
            }
        }

        [DisplayName("Search Name")]
        public string SearchName { get; set; }
    }
}
