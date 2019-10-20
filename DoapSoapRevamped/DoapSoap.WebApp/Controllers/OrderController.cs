using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.DataAccess.Repositories;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoapSoap.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private ICustomerRepository _crepo;
        private ILocationRepository _lrepo;
        private CartViewModel _cart;


        public OrderController(ICustomerRepository crepo, ILocationRepository lrepo, CartViewModel cart)
        {
            _crepo = crepo;
            _lrepo = lrepo;
            _cart = cart;
        }

        public IActionResult PlaceOrder()
        {
            var viewmodel = new PlaceOrderViewModel
            {
                locations = _lrepo.GetAllLocations(),
                customers = _crepo.GetAllCustomers(),
                selectedOptions = false
            };

            _cart.Cart.Add(new BusinessLogic.Models.Product { Name = "fried chicken"},1);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(PlaceOrderViewModel model)
        {
            model.selectedCustomer = model.selectedCustomer;
            model.selectedOptions = true;
            return View(model);
        }
    }
}