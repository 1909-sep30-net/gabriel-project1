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

        public IEnumerable<Location> GetAllLocations()
        {
            return _context.Locations.AsNoTracking().Select(Mapper.MapLocationWithoutOI);
        }

        /// <summary>
        /// Get locations with everything (order items, inventory items)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Location GetLocation(int id)
        {
            return Mapper.MapLocation(_context.Locations.Find(id));
        }

        public IDictionary<Product, int> GetLocationInventory(Location location)
        {
            //return _context.Locations.Where(l=>l.LocationId == location.ID)
            //    .Include(l=>l.InventoryItems)
            //    .First()
            //    .InventoryItems.ToDictionary(ii=>MapProduct(ii.Product), ii=>ii.Quantity);
            var newLocation = _context.Locations
                .Where(l => l.LocationId == location.ID)
                .Include(l => l.InventoryItems)
                .First();

            return Mapper.MapLocation(newLocation).Inventory;
        }

        public IEnumerable<Order> GetOrders(Location location)
        {
            throw new NotImplementedException();
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
