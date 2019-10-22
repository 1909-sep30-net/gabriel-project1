using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Http;
using DoapSoap.BusinessLogic.Interfaces;

namespace DoapSoap.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocationRepository _lrepo;

        public HomeController(ILogger<HomeController> logger, ILocationRepository lrepo)
        {
            _logger = logger;
            _lrepo = lrepo;
        }

        public IActionResult Index()
        {
            RecordInSession("Home");

            _logger.LogInformation("Went to home page");

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

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private void RecordInSession(string action)
        {
            var paths = HttpContext.Session.GetString("actions") ?? string.Empty;
            HttpContext.Session.SetString("actions", paths + ";" + action);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
