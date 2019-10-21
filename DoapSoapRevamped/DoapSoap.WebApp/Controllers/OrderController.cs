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

            var cart = HttpContext.Session.GetObject<CustomerViewModel>("Cart");
            cart.FirstName += "BILLY";

            HttpContext.Session.SetObject("Cart", cart);

            model.Products = viewmodel;

            var addProductModel = new AddProductViewModel
            {
                SelectedCustomer = model.selectedCustomer,
                SelectedLocation = model.selectedLocation
            };
            //model.selectedLocation.Inventory
            model.selectedLocation.Inventory = _lrepo.GetLocationInventory(locId);
            HttpContext.Session.SetObject("SelectedCustomer",model.selectedCustomer);
            HttpContext.Session.SetObject("SelectedLocation", model.selectedLocation);

            return RedirectToAction(nameof(AddProducts));
        }

        /// <summary>
        /// Takes you to view where you can start adding products to your order
        /// </summary>
        /// <returns></returns>
        public IActionResult AddProducts()
        {
            var selcus = HttpContext.Session.GetObject<BusinessLogic.Models.Customer>("SelectedCustomer");
            var selloc = HttpContext.Session.GetObject<BusinessLogic.Models.Location>("SelectedLocation");
            var model = new AddProductViewModel
            {
                SelectedCustomer = selcus,
                SelectedLocation = selloc,
                
            };
            return View(model);
        }
    }
}