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
            var newEntity = Mapper.MapCustomer(customer);
            newEntity.CustomerId = 0;
            _context.Add(newEntity);
            _context.SaveChanges();
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
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Spice)
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


    }
}
