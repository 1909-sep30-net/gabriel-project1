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

        public OrderController(ICustomerRepository crepo, ILocationRepository lrepo, CartViewModel cart)
        {
            _crepo = crepo;
            _lrepo = lrepo;
        }

        public IActionResult PlaceOrder()
        {

            var viewmodel = new PlaceOrderViewModel
            {
                locations = _lrepo.GetAllLocations(),
                customers = _crepo.GetAllCustomers(),
                selectedOptions = false,
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(PlaceOrderViewModel model)
        {
            // locId and custId should hold the id of the selected from before
            int locId = int.Parse(Request.Form["sel1"]);
            int custId = int.Parse(Request.Form["sel2"]);
            model.selectedLocation = _lrepo.GetLocation(locId);
            model.selectedCustomer = _crepo.GetCustomer(custId);

            model.locations = _lrepo.GetAllLocations();
            model.customers = _crepo.GetAllCustomers();

            model.selectedOptions = true;

            var BLmodel = _lrepo.GetLocationInventory(locId);
            var viewmodel = BLmodel.Select(i => new LocationInventoryViewModel
            {
                ProductName = i.Key.Name,
                Spice = i.Key.Spice.Name,
                Price = i.Key.Price,
                Quantity = i.Value
            });

            model.Products = viewmodel;

            return View(model);
        }
    }
}