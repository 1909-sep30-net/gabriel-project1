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
        private readonly IStoreRepository _repo;

        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public CustomerController(IStoreRepository repo)
        {
            _repo = repo;
        }

        // Display list of customers
        public ActionResult Index()
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

        // Display specific customer's Order History and details of order
        public ActionResult Details(int id)
        {
            IEnumerable<BusinessLogic.Models.Order> orders = _repo.GetOrdersByCustomerID(id);

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
                _repo.Add(newCustomer);
                logger.Info("Added new customer");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                return View(viewModel);
            }
        }


        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}