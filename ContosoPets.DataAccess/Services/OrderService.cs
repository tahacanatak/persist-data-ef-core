using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoPets.DataAccess.Data;
using ContosoPets.Domain.DataTransferObjects;
using ContosoPets.Domain.Models;


namespace ContosoPets.DataAccess.Services
{
    public class OrderService
    {
        private readonly ContosoPetsContext _context;

        public OrderService(ContosoPetsContext context)
        {
            _context = context;
        }

       public async Task<List<CustomerOrder>> GetAll()
       {
           List<CustomerOrder> orders = await (_context.Orders.AsNoTracking()
           .OrderByDescending(o => o.OrderPlaced)
           .Select(o => new CustomerOrder
           {
               OrderId = o.Id,
               CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
               OrderFulfilled = o.OrderFulfilled.HasValue ?
               o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
               OrderPlaced = o.OrderPlaced.ToShortDateString(),
               OrderLineItems = (o.ProductOrders.Select(po => new OrderLineItem
               {
                   ProductQuantity = po.Quantity,
                   ProductName = po.Product.Name
               }))
           })).ToListAsync();

           return orders;
       }
    }
}