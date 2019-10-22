using DoapSoap.BusinessLogic.Interfaces;
using DoapSoap.BusinessLogic.Models;
using DoapSoap.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DoapSoapContext _context;
        public OrderRepository(DoapSoapContext context)
        {
            _context = context ?? throw new NullReferenceException("Context cannot be null!");
        }
        public void AddOrder(Order order)
        {
            var entity = Mapper.MapOrder(order);
            _context.Add(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
