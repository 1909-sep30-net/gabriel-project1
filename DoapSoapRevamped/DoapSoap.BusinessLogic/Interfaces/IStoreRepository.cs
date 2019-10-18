using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Interfaces
{
    public interface IStoreRepository : IDisposable
    {
        void Add(Customer customer);
        void Add(Order order);

        Customer GetCustomer(int id);
        Location GetLocation(int id);
        /// <summary>
        /// Get customer's history of orders
        /// </summary>
        /// <param name="id">ID of the customer</param>
        /// <returns>List of orders from specified customer</returns>
        IEnumerable<Order> GetOrdersByCustomerID(int id);
        /// <summary>
        /// Get location's history of orders
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns>List of orders from specified location</returns>
        IEnumerable<Order> GetOrdersByLocationID(int id);

        /// <summary>
        /// Get a list of all customers, or a list of matching customers to a name
        /// </summary>
        /// <param name="name">Optional string search for matching customer name</param>
        /// <returns></returns>
        IEnumerable<Customer> GetAllCustomers(string name = null);
        
        /// <summary>
        /// Get a list of all locations
        /// </summary>
        /// <returns></returns>
        IEnumerable<Location> GetAllLocations();

        void UpdateLocationInventory(Location location);
    }
}
