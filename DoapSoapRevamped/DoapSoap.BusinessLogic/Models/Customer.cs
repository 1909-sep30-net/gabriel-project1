using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public ISet<Order> OrderHistory { get; set; }
    }
}
