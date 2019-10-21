using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Http;

namespace DoapSoap.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            RecordInSession("Home");
            var mockCustomer = new CustomerViewModel
            {
                FirstName = "ima append billy to this and reset every time i go to the Home page --",
                Phone = "6268075313"
            };
            HttpContext.Session.SetObject("Cart", mockCustomer);

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
