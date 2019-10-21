using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Interfaces
{
    public interface ICustomerRepository : IDisposable
    {
        /// <summary>
        /// Adds a customer to the database
        /// </summary>
        /// <param name="customer">Customer to add to database</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Get a customer(by ID) from the database without order history
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Customer GetCustomer(int id);

        /// <summary>
        /// Get customer's history of orders
        /// </summary>
        /// <param name="customer">the customer we want to retrieve orders for</param>
        /// <returns>List of orders from specified customer</returns>
        IEnumerable<Order> GetOrdersWithProductDetails(int id);

        /// <summary>
        /// Get a list of all customers, or a list of matching customers to a name
        /// </summary>
        /// <param name="name">Optional string search for matching customer name</param>
        /// <returns></returns>
        IEnumerable<Customer> GetAllCustomers(string name = null);

        Product GetProduct(int id);

    }
}
