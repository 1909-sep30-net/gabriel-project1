using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoapSoap.WebApp.Models
{
    public class CartViewModel
    {
        public IDictionary<Product,int> Cart { get; set; }
    }
}
