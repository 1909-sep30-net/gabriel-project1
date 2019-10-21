using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Interfaces
{
    public interface ILocationRepository
    {

        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Location GetLocation(int id);

        /// <summary>
        /// Get location with inventory populated
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Location GetLocationWithInventory(int id);

        /// <summary>
        /// Get location's history of orders
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns>List of orders from specified location</returns>
        IEnumerable<Order> GetOrdersWithProductDetails(int id);

        /// <summary>
        /// Get a list of all locations without including
        /// </summary>
        /// <returns></returns>
        IEnumerable<Location> GetAllLocations();

        /// <summary>
        /// Get a dictionary of type key-product,value-int that represents the inventory of the location
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns></returns>
        IDictionary<Product, int> GetLocationInventory(int id);


        /// <summary>
        /// Edit/updates the location's inventory to the database
        /// </summary>
        /// <param name="location"></param>
        void UpdateLocationInventory(Location location);

        void SaveChanges();
    }
}
