﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoapSoap.BusinessLogic.Models;
using DoapSoap.DataAccess.Entities;

namespace DoapSoap.DataAccess
{
    public static class Mapper
    {
        public static BusinessLogic.Models.Location MapLocation(Entities.Locations location)
        {
            return new BusinessLogic.Models.Location
            {
                ID = location.LocationId,
                Name = location.Name,
                OrderHistory = location.Orders.Select(MapOrder).ToHashSet(),
                Inventory = location.InventoryItems.ToDictionary(o => MapProduct(o.Product), o => o.Quantity)
            };
        }

        /// <summary>
        /// Map location without order history
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static BusinessLogic.Models.Location MapLocationWithoutOH(Entities.Locations location)
        {
            return new BusinessLogic.Models.Location
            {
                ID = location.LocationId,
                Name = location.Name,
                Inventory = location.InventoryItems.ToDictionary(o => MapProduct(o.Product), o => o.Quantity)
            };
        }

        public static BusinessLogic.Models.Location MapLocationWithoutOI(Entities.Locations location)
        {
            return new BusinessLogic.Models.Location
            {
                ID = location.LocationId,
                Name = location.Name,
            };
        }

        public static Entities.Locations MapLocation(BusinessLogic.Models.Location location)
        {
            return new Entities.Locations
            {
                LocationId = location.ID,
                Name = location.Name
            };
        }

        public static Entities.Locations MapLocationWithInventory(BusinessLogic.Models.Location location)
        {
            return new Entities.Locations
            {
                LocationId = location.ID,
                Name = location.Name,
                InventoryItems = MapInventoryItems(location)
            };
        }

        public static BusinessLogic.Models.Customer MapCustomer(Entities.Customers customer)
        {
            return new BusinessLogic.Models.Customer
            {
                ID = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                OrderHistory = customer.Orders.Select(MapOrder).ToList()
            };
        }

        public static BusinessLogic.Models.Customer MapCustomerWithoutOrders(Entities.Customers customer)
        {
            return new BusinessLogic.Models.Customer
            {
                ID = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
            };
        }

        public static Entities.Customers MapCustomer(BusinessLogic.Models.Customer customer)
        {
            return new Entities.Customers
            {
                CustomerId = customer.ID,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone
            };
        }

        /// <summary>
        /// Map Orders with unpopulated customer/location
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static BusinessLogic.Models.Order MapOrder(Entities.Orders order)
        {
            return new BusinessLogic.Models.Order
            {
                ID = order.OrderId,
                TimePlaced = order.TimeConfirmed,
                ProductList = order.OrderItems.ToDictionary(o => MapProduct(o.Product), o => o.Quantity),
                Customer = MapCustomerWithoutOrders(order.Customer),
                Location = MapLocationWithoutOI(order.Location)
            };
        }
        public static Entities.Orders MapOrder(BusinessLogic.Models.Order order)
        {
            return new Entities.Orders
            {
                OrderId = order.ID,
                LocationId = order.Location.ID,
                CustomerId = order.Customer.ID,
                TimeConfirmed = order.TimePlaced,
                OrderItems = MapOrderItems(order.ProductList)
            };
        }
        public static HashSet<Entities.OrderItems> MapOrderItems(Dictionary<BusinessLogic.Models.Product, int> pl)
        {
            HashSet<Entities.OrderItems> OIList = new HashSet<Entities.OrderItems>();
            foreach (KeyValuePair<BusinessLogic.Models.Product, int> key in pl)
            {
                OIList.Add(new Entities.OrderItems
                {
                    ProductId = key.Key.ID,
                    //Product = MapProduct(key.Key),
                    Quantity = key.Value
                });
            }
            return OIList;
        }

        public static ICollection<Entities.InventoryItems> MapInventoryItems(Location location)
        {
            var inventory = location.Inventory;
            List<InventoryItems> II = new List<InventoryItems>();
            foreach (var item in inventory)
            {
                II.Add(new InventoryItems
                {
                    LocationId = location.ID,
                    ProductId = item.Key.ID,
                    Quantity = item.Value
                });
            }
            return II;
        }

        public static BusinessLogic.Models.Product MapProduct(Entities.Products product)
        {
            return new BusinessLogic.Models.Product
            {
                ID = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Spice = MapSpice(product.Spice)
            };
        }

        public static Entities.Products MapProduct(BusinessLogic.Models.Product product)
        {
            //TODO: check if we actually need the rest of the values when adding orders back into db
            return new Entities.Products
            {
                ProductId = product.ID,
                Spice = MapSpice(product.Spice)
            };
        }

        public static BusinessLogic.Models.SpiceLevel MapSpice(Entities.SpiceLevels spice)
        {
            return new BusinessLogic.Models.SpiceLevel
            {
                ID = spice.SpiceId,
                Name = spice.Name
            };
        }

        public static Entities.SpiceLevels MapSpice(BusinessLogic.Models.SpiceLevel spice)
        {
            return new Entities.SpiceLevels
            {
                SpiceId = spice.ID
            };
        }
    }
}
