using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.BusinessLogic.Models;
using DoapSoap.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoapSoap.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {

        private readonly DoapSoapContext _context;

        /// <summary>
        /// Construct repository with the database context
        /// </summary>
        /// <remarks>
        /// Repository is required to be instantiated with a context or else!! throw an exception (so we dont have to handle that in every method call)
        /// </remarks>
        /// <param name="context"></param>
        public CustomerRepository(DoapSoapContext context)
        {
            _context = context ?? throw new NullReferenceException("Context cannot be null!");
        }

        /// <summary>
        /// Adds a customer to the database
        /// </summary>
        /// <param name="customer">Customer to add to database</param>
        public void AddCustomer(Customer customer)
        {
            
            var newEntity = Mapper.MapCustomer(customer);

            // Make sure customer id is set to 0 since this is SUPPOSED TO BE a new customer
            newEntity.CustomerId = 0;

            _context.Add(newEntity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get a customer(by ID) from the database without order history
        /// </summary>
        /// <param name="id"> id of the customer </param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            return Mapper.MapCustomer(_context.Customers.Find(id));
        }

        /// <summary>
        /// Get a list of all customers, or a list of matching customers to a name
        /// </summary>
        /// <param name="name">Optional string search for matching customer name</param>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers(string name = null)
        {
            // If a name is provided, search for customers that have names that contain the given string
            if (name != null)
            {
                return _context.Customers
                    .Where(c => (c.FirstName + c.LastName).Contains(name))
                    .Select(Mapper.MapCustomer);
            }

            // Otherwise, return the whole list of customers, mapped
            return _context.Customers.Select(Mapper.MapCustomer).ToList();

        }

        /// <summary>
        /// Get customer's history of orders
        /// </summary>
        /// <param name="customer">the customer we want to retrieve orders for</param>
        /// <returns>List of orders from specified customer</returns>
        public IEnumerable<Order> GetCustomersOrders(Customer customer)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Location)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Spice)
                .Where(o => o.CustomerId == customer.ID)
                .Select(Mapper.MapOrder).ToList();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
