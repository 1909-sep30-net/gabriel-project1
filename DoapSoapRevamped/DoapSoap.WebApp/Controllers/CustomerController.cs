using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.DataAccess.Repositories;
using DoapSoap.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DoapSoap.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _repo;

        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public CustomerController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        // Display list of customers
        public ActionResult AllCustomers()
        {
            IEnumerable<BusinessLogic.Models.Customer> customers = _repo.GetAllCustomers(null);

            var viewmodels = customers.Select(c => new CustomerViewModel
            {
                Id = c.ID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone
            });

            logger.Info("Viewing Customers");

            return View(viewmodels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllCustomers(string SearchName)
        {
            // Redirect from customer page to search customer page with a string (the customer name to be searched)
            return RedirectToAction("Search", new { search = SearchName });
        }

        // Display specific customer's Order History and details of order
        public ActionResult OrderHistory(int id)
        {
            IEnumerable<BusinessLogic.Models.Order> orders = _repo.GetOrdersWithProductDetails(id);

            string customerName = _repo.GetCustomer(id).Name;

            ViewData["CustomerLogName"] = customerName;

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

            logger.Info($"Viewing {customerName}'s Order History");

            return View(viewmodels);
        }



        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel viewModel)
        {
            try
            {
                // Create new instance of customer from the viewmodel
                var newCustomer = new BusinessLogic.Models.Customer
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Phone = viewModel.Phone
                };

                // Add to db via repo
                _repo.AddCustomer(newCustomer);
                logger.Info("Added new customer");

                return RedirectToAction(nameof(AllCustomers));
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                return View(viewModel);
            }
        }

        public ActionResult Search(string search)
        {
            var customers = _repo.GetAllCustomers(search);
            ViewData["SearchName"] = search;
            return View(customers);
        }
    }
}