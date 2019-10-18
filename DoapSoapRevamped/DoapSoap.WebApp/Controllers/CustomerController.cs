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

        // GET: Customer
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

        // Get Customer Order History
        public ActionResult Details(int id)
        {
            IEnumerable<BusinessLogic.Models.Order> orders = _repo.GetOrdersByCustomerID(id);
            List<OrderHistoryViewModel> viewmodels = new List<OrderHistoryViewModel>();
            var customer = _repo.GetCustomer(id);
            if (orders.Count() > 0)
            {
                viewmodels = orders.Select(c => new OrderHistoryViewModel
                {
                    Id = c.ID,
                    LocationName = c.Location.Name,
                    Time = c.TimePlaced,
                    CustomerName = c.Customer.Name,
                    Customer = c.Customer
                }).ToList();
            } 
            else
            {
                viewmodels.Add(new OrderHistoryViewModel
                {
                    Id = 0,
                    LocationName = "",
                    Time = DateTime.Now,
                    CustomerName = customer.Name,
                    Customer = customer
                });
            }


            logger.Info($"Viewing {_repo.GetCustomer(id).Name} Order History");

            return View(viewmodels);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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