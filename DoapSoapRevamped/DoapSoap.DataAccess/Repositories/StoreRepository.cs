using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.BusinessLogic.Models;
using DoapSoap.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoapSoap.DataAccess.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DoapSoapContext _context;

        public StoreRepository()
        {

        }
        public StoreRepository(DoapSoapContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void Add(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers(string name = null)
        {
            return _context.Customers.Select(Mapper.MapCustomer).ToList();
        }

        public IEnumerable<Location> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            return Mapper.MapCustomer(_context.Customers.Find(id));
            throw new NotImplementedException();
        }

        public Location GetLocation(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrdersByCustomerID(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Location)
                .Where(o => o.CustomerId == id)
                .Select(Mapper.MapOrder).ToList();
        }

        public IEnumerable<Order> GetOrdersByLocationID(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateLocationInventory(Location location)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StoreRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
