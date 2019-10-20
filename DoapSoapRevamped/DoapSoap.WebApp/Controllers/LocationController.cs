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
        private readonly ILocationRepository _repo;

        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LocationController(ILocationRepository repo)
        {
            logger.Info("Location controller instantiated.");
            _repo = repo ?? throw new ArgumentNullException("Repository cannot be null",nameof(repo));
        }

        /// <summary>
        /// Returns a view of all available locations
        /// </summary>
        /// <returns></returns>
        public IActionResult AllLocations()
        {
            var viewmodels = _repo.GetAllLocations();
            return View(viewmodels);
        }

        /// <summary>
        /// Returns a view of the selected location's order history
        /// </summary>
        /// <param name="id">ID of the desired location</param>
        /// <returns></returns>
        public IActionResult OrderHistory(int id)
        {
            var location = _repo.GetLocation(id);
            ViewData["SelectedLocName"] = location.Name;

            IEnumerable<BusinessLogic.Models.Order> orders = _repo.GetOrdersWithProductDetails(id);

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
            var location = _repo.GetLocation(id);
            var BLmodel = _repo.GetLocationInventory(id);
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