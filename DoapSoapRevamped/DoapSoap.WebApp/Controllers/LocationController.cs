using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoapSoap.WebApp.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult AllLocations()
        {
            return View();
        }
    }
}