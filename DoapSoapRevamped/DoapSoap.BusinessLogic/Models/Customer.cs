using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Customer
    {
        private string _phone;
        private string _firstName;
        private string _lastName;

        /// <summary>
        /// Handles getting and setting first name
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                // Always capitalize first letter and lowercase the rest
                _firstName = value[0].ToString().ToUpper() + value.Substring(1).ToLower();
            }
        }

        /// <summary>
        /// Handles getting and setting last name
        /// </summary>
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                // Always capitalize first letter and lowercase the rest
                _lastName = value[0].ToString().ToUpper() + value.Substring(1).ToLower();
            }
        }
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
