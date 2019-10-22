using DoapSoap.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoapSoap.BusinessLogic.Interfaces
{
    public interface IOrderRepository : IDisposable
    {
        /// <summary>
        /// Add order to database
        /// </summary>
        /// <param name="order"></param>
        void AddOrder(Order order);

        /// <summary>
        /// Save local changes to database
        /// </summary>
        void SaveChanges();
    }
}
