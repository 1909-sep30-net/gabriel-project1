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
    public class LocationRepository : ILocationRepository, IDisposable
    {
        private readonly DoapSoapContext _context;

        public LocationRepository(DoapSoapContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Grabs a list of all locations but only their names + ids
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Location> GetAllLocations()
        {
            // We shouldnt be modifying any of these locations, so mark as no tracking
            return _context.Locations.AsNoTracking().Select(Mapper.MapLocationWithoutOI);
        }

        /// <summary>
        /// Get locations without everything (order items, inventory items)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Location GetLocation(int id)
        {
            return Mapper.MapLocation(_context.Locations.Find(id));
        }

        public IDictionary<Product, int> GetLocationInventory(int id)
        {
            //return _context.Locations.Where(l=>l.LocationId == location.ID)
            //    .Include(l=>l.InventoryItems)
            //    .First()
            //    .InventoryItems.ToDictionary(ii=>MapProduct(ii.Product), ii=>ii.Quantity);
            var newLocation = _context.Locations
                .Where(l => l.LocationId == id)
                .Include(l => l.InventoryItems)
                    .ThenInclude(i=>i.Product)
                        .ThenInclude(p=>p.Spice)
                .First();

            return Mapper.MapLocation(newLocation).Inventory;
        }

        public IEnumerable<Order> GetOrdersWithProductDetails(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Location)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Spice)
                .AsNoTracking()
                .Where(o => o.LocationId == id)
                .Select(Mapper.MapOrder)
                .ToList();
        }


        public void UpdateLocationInventory(Location location)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
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
