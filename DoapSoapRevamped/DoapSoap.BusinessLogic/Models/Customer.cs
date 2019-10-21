using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Customer
    {
        private string _phone;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public string Name 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value.Length != 10)
                {
                    throw new ArgumentException("Phone must be valid 10 digit number.", nameof(value));
                }

                _phone = value;
            }
        }

        public string DisplayPhone
        {
            get
            {
                return String.Format("({0}) {1}-{2}", _phone.Substring(0, 3), _phone.Substring(3, 3), _phone.Substring(6, 4));
            }
        }

        public IEnumerable<Order> OrderHistory { get; set; } = new List<Order>();
    }
}
