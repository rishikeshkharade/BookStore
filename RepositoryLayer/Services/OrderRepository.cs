using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IHttpContextAccessor _http;

        public OrderRepository(BookStoreDbContext ctx, IHttpContextAccessor http)
        {
            _context = ctx;
            _http = http;
        }

        public Order CreateOrderFromCart()
        {
            var userId = int.Parse(_http.HttpContext.User.FindFirst("Id").Value);

            // Grab cart items
            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
                throw new Exception("Cart is empty");

            // Build the Order + OrderItems
            var order = new Order
            {
                UserId = userId,
                PurchaseAt = DateTime.Now,
                Items = cartItems.Select(c => new OrderItem
                {
                    BookId = c.BookId,
                    Quantity = c.Quantity,
                    Price = c.Price
                }).ToList()
            };

            // Persist
            _context.Orders.Add(order);
            // Clear the cart
            _context.Carts.RemoveRange(cartItems);
            _context.SaveChanges();

            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            var userId = int.Parse(_http.HttpContext.User.FindFirst("Id").Value);
            return _context.Orders
                           .Include(o => o.Items)
                           .Where(o => o.UserId == userId)
                           .ToList();
        }
    }
}
