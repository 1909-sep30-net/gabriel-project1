using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DoapSoap.WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepository _lrepo;

        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LocationController(ILocationRepository repo)
        {
            logger.Info("Location controller instantiated.");
            _lrepo = repo ?? throw new ArgumentNullException("Repository cannot be null",nameof(repo));
        }

        /// <summary>
        /// Returns a view of all available locations
        /// </summary>
        /// <returns></returns>
        public IActionResult AllLocations()
        {
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
                    var product = location.Inventory.Where(i => i.Key.ID == item.Key).First().Key;
                    location.Inventory[product] += item.Value;
                }
                // Save changes to the database
                _lrepo.UpdateLocationInventory(location);
                _lrepo.SaveChanges();
            }

            var viewmodels = _lrepo.GetAllLocations();
            return View(viewmodels);
        }

        /// <summary>
        /// Returns a view of the selected location's order history
        /// </summary>
        /// <param name="id">ID of the desired location</param>
        /// <returns></returns>
        public IActionResult OrderHistory(int id)
        {
            var location = _lrepo.GetLocation(id);
            ViewData["SelectedLocName"] = location.Name;

            IEnumerable<BusinessLogic.Models.Order> orders = _lrepo.GetOrdersWithProductDetails(id);

            // Populate each order history view with their own list of product details
            var viewmodels = orders.Select(c => new OrderHistoryViewModel
            {
                Id = c.ID,
                LocationName = c.Location.Name,
                Time = c.TimePlaced,
                CustomerName = c.Customer.Name,
                ProductList = c.ProductList.Select(i => new OrderDetailsViewModel
                {
                    ProductName = i.Key.Name,
                    Spice = i.Key.Spice.Name,
                    Quantity = i.Value,
                    Price = i.Key.Price
                })
            });

            logger.Info($"Viewing {location.Name}'s Order History");

            return View(viewmodels);
        }

        /// <summary>
        /// Returns a view of the selected location's inventory
        /// </summary>
        /// <param name="id">ID of the desired location</param>
        /// <returns></returns>
        public ActionResult Inventory(int id)
        {
            var location = _lrepo.GetLocation(id);
            var BLmodel = _lrepo.GetLocationInventory(id);
            var viewmodel = BLmodel.Select(i => new LocationInventoryViewModel {
                ProductName = i.Key.Name,
                Spice = i.Key.Spice.Name,
                Price = i.Key.Price,
                Quantity = i.Value
            });

            ViewData["SelectedLocName"] = location.Name;

            return View(viewmodel);
        }
    }
}