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
        private IOrderRepository _orepo;
        //private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();


        public OrderController(ICustomerRepository crepo, ILocationRepository lrepo, IOrderRepository orepo)
        {
            _crepo = crepo;
            _lrepo = lrepo;
            _orepo = orepo;
        }

        public IActionResult PlaceOrder()
        {

            var viewmodel = new PlaceOrderViewModel
            {
                locations = _lrepo.GetAllLocations(),
                customers = _crepo.GetAllCustomers(),
                selectedOptions = false,
            };

            // If we come to this page but the cart isn't empty, we need to put the products back into their respective place
            if (HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") != null)
            {
                // Get existing cart, location it was taken from
                var cart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart");
                var locationID = HttpContext.Session.GetObject<int>("SelectedLocationID");
                var location = _lrepo.GetLocation(locationID);
                location.Inventory = _lrepo.GetLocationInventory(locationID);

                // Increase location inventory quantities corresponding with the cart
                foreach (var item in cart)
                {
                    var product = location.Inventory.Where(i=>i.Key.ID == item.Key).First().Key;
                    location.Inventory[product] += item.Value;
                }
                // Save changes to the database
                _lrepo.UpdateLocationInventory(location);
                _lrepo.SaveChanges();
            }

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
            };

            HttpContext.Session.SetObject("SelectedLocationID", locID);
            HttpContext.Session.SetObject("SelectedCustomerID", custID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProducts(AddProductViewModel model)
        {


            int locID = (int)TempData["SelectedLocID"];
            int custID = (int)TempData["SelectedCustID"];

            var location = _lrepo.GetLocation(locID);
            var customer = _crepo.GetCustomer(custID);
            var oldInventory = _lrepo.GetLocationInventory(locID);
            location.Inventory = oldInventory;

            // Grab the product and amount selected, then add to cart
            int quantity = model.SelectedQuantity;
            var product = oldInventory.Keys.Where(p => p.ID == model.SelectedProductID).First();

            // Update inventory with model's selected quantity and location
            try
            {
                location.RemoveFromInventory(product, quantity);
                _lrepo.UpdateLocationInventory(location);
                _lrepo.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Could not remove from inventory");

                // Recreate the cart to display without decrementing the inventory
                var sameCart = new Dictionary<int, int>();
                var sameDisplayCart = new Dictionary<Product, int>();
                var sameCartVM = new List<CartViewModel>();

                // If there's stuff in the cart, populate
                if (HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") != null)
                {
                    var currentCart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart");
                    sameCart = new Dictionary<int, int>(currentCart);

                    foreach (var item in sameCart)
                    {
                        var newProduct = _crepo.GetProduct(item.Key);
                        sameDisplayCart.Add(newProduct, item.Value);
                        sameCartVM.Add(new CartViewModel
                        {
                            ProductName = newProduct.Name,
                            Price = newProduct.Price,
                            Quantity = item.Value,
                            SpiceLevel = newProduct.Spice.Name
                        });
                    }
                }

                var returnModel = new AddProductViewModel
                {
                    SelectedCustomer = customer,
                    SelectedLocation = location,
                    DisplayCart = sameDisplayCart,
                    Inventory = location.Inventory,
                    _Cart = sameCartVM,
                    InvalidQuantityTaken = true
                };
                return View(returnModel);
            }

            // Configure new cart
            var newCart = new Dictionary<int, int>();

            if (HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") != null)
            {
                var currentCart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart");
                newCart = new Dictionary<int, int>(currentCart);
            }
            try
            {
                // Add new product id to dictionary
                newCart.Add(product.ID, quantity);
            }
            catch
            {
                // Catch if product is already in dictionary, then just increase the quantity
                newCart[product.ID] += quantity;
            }
            HttpContext.Session.SetObject("Cart", newCart);

            // populate display cart using our persistent cart
            var newDisplayCart = new Dictionary<Product, int>();
            var newCartVM = new List<CartViewModel>();

            foreach (var item in newCart)
            {
                var newProduct = _crepo.GetProduct(item.Key);
                newDisplayCart.Add(newProduct, item.Value);
                newCartVM.Add(new CartViewModel
                {
                    ProductName = newProduct.Name,
                    Price = newProduct.Price,
                    Quantity = item.Value,
                    SpiceLevel = newProduct.Spice.Name
                });
            }


            // Model we're sending to view
            var newModel = new AddProductViewModel
            {
                SelectedCustomer = customer,
                SelectedLocation = location,
                DisplayCart = newDisplayCart,
                Inventory = location.Inventory,
                _Cart = newCartVM
            };

            return View(newModel);
        }

        /// <summary>
        /// Takes cart, customer id, and store location and populates an order object, then attempts to add to db
        /// </summary>
        /// <returns></returns>
        public IActionResult ConfirmOrder()
        {
            // Translate the cart into the order inventory
            Dictionary<Product, int> orderInventory = new Dictionary<Product, int>();
            var cart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart");
            foreach (var item in cart)
            {
                var product = _crepo.GetProduct(item.Key);
                orderInventory.Add(product,item.Value);
            }
            // Empty cart into an order
            Order order = new Order
            {
                TimePlaced = DateTime.Now,
                Customer = _crepo.GetCustomer(HttpContext.Session.GetObject<int>("SelectedCustomerID")),
                Location = _lrepo.GetLocation(HttpContext.Session.GetObject<int>("SelectedLocationID")),
                ProductList = orderInventory
            };

            _orepo.AddOrder(order);
            _orepo.SaveChanges();

            var cartPreview = new List<CartViewModel>();
            foreach (var item in cart)
            {
                var newProduct = _crepo.GetProduct(item.Key);
                cartPreview.Add(new CartViewModel
                {
                    ProductName = newProduct.Name,
                    Price = newProduct.Price,
                    Quantity = item.Value,
                    SpiceLevel = newProduct.Spice.Name
                });
            }

            return View(cartPreview);
        }
    }
}