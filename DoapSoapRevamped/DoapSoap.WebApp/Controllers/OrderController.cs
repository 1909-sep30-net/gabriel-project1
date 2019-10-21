using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.BusinessLogic.Models;
using DoapSoap.DataAccess.Repositories;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DoapSoap.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private ICustomerRepository _crepo;
        private ILocationRepository _lrepo;
        //private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();


        public OrderController(ICustomerRepository crepo, ILocationRepository lrepo)
        {
            _crepo = crepo;
            _lrepo = lrepo;
            //_logger = logger;
        }

        public IActionResult PlaceOrder()
        {

            var viewmodel = new PlaceOrderViewModel
            {
                locations = _lrepo.GetAllLocations(),
                customers = _crepo.GetAllCustomers(),
                selectedOptions = false,
            };
            HttpContext.Session.SetObject("Cart", null);

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

            var addProductModel = new AddProductViewModel
            {
                SelectedCustomer = model.selectedCustomer,
                SelectedLocation = model.selectedLocation
            };

            model.selectedLocation.Inventory = _lrepo.GetLocationInventory(locId);
            TempData["SelectedCustID"] = model.selectedCustomer.ID;
            TempData["SelectedLocID"] = model.selectedLocation.ID;


            return RedirectToAction(nameof(AddProducts));
        }



        /// <summary>
        /// Takes you to view where you can start adding products to your order
        /// </summary>
        /// <returns></returns>
        public IActionResult AddProducts()
        {
            int locID = (int)TempData["SelectedLocID"];
            int custID = (int)TempData["SelectedCustID"]; 

            var location = _lrepo.GetLocation(locID);
            var locationInv = _lrepo.GetLocationInventory(location.ID);

            var customer = _crepo.GetCustomer(custID);

            var newHiddenInventory = new Dictionary<int, int>();
            foreach (var item in locationInv)
            {
                newHiddenInventory.Add(item.Key.ID, item.Value);
            }

            var model = new AddProductViewModel
            {
                SelectedCustomer = customer,
                SelectedLocation = location,
                Inventory = locationInv,
                HiddenInventory = newHiddenInventory
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProducts(AddProductViewModel model)
        {
            // Update returned inventory with model's selected quantity and location


            int locID = (int)TempData["SelectedLocID"];
            int custID = (int)TempData["SelectedCustID"];

            var location = _lrepo.GetLocation(locID);
            var customer = _crepo.GetCustomer(custID);

            // Grab the product and amount selected, then add to cart
            int quantity = model.SelectedQuantity;

            // Get location and populate inventory
            var pastInventory = ViewBag.HiddenInventory as Dictionary<int, int>;
            var currentInventory = new Dictionary<Product, int>();
            foreach(var item in pastInventory)
            {
                var newProduct = _crepo.GetProduct(item.Key);
                currentInventory.Add(newProduct, item.Value);
            }

            location.Inventory = currentInventory;
            var product = location.Inventory.Keys.Where(p => p.ID == model.SelectedProductID).First();

            try
            {
                location.RemoveFromInventory(product, quantity);
            }
            catch
            {
                Console.WriteLine("Could not remove from inventory");
            }

            // Copy updated inventory into HiddenInventory tempdata
            var newHiddenInv = new Dictionary<int, int>();
            foreach(var item in location.Inventory)
            {
                newHiddenInv.Add(item.Key.ID,item.Value);
            }
            ViewBag.HiddenInventory = newHiddenInv;

            // Configure new cart
            var newCart = new Dictionary<int, int>();

            if (HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") != null)
            {
                var currentCart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart");
                newCart = new Dictionary<int, int>(currentCart);
            }
            newCart.Add(product.ID, quantity);
            HttpContext.Session.SetObject("Cart", newCart);

            // populate display cart using our persistent cart
            var newDisplayCart = new Dictionary<BusinessLogic.Models.Product, int>();
            foreach (var item in newCart)
            {
                var newProduct = _crepo.GetProduct(item.Key);
                newDisplayCart.Add(newProduct, item.Value);
            }

            // Model we're sending to view
            var newModel = new AddProductViewModel
            {
                SelectedCustomer = customer,
                SelectedLocation = location,
                DisplayCart = newDisplayCart,
                Inventory = location.Inventory,
                HiddenInventory = newHiddenInv,
            };

            return View(newModel);
        }

        /// <summary>
        /// Takes cart, customer id, and store location and populates an order object, then attempts to add to db
        /// </summary>
        /// <returns></returns>
        public IActionResult ConfirmOrder()
        {
            return RedirectToRoute("default");
        }
    }
}