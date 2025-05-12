using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(
            BookStoreDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart AddToCart(CartRequestModel model)
        {
            // get userId from JWT claim "Id"
            var user = _httpContextAccessor.HttpContext.User;
            var claim = user.FindFirst("Id")
                     ?? throw new Exception("UserId claim missing");
            if (!int.TryParse(claim.Value, out var userId))
                throw new Exception("Invalid UserId claim");

            // lookup book price
            var book = _context.Books.Find(model.BookId)
                       ?? throw new Exception("Book not found");

           var existing = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.BookId == model.BookId);
            if (existing != null)
            {
                existing.Quantity++;
                _context.SaveChanges();
                return existing;
            }
            var cart = new Cart
            {
                UserId = userId,
                BookId = model.BookId,
                Quantity = 1,
                Price = book.Price
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart;
        }

        public IEnumerable<Cart> GetCartItems()
        {
            var userId = int.Parse(
              _httpContextAccessor.HttpContext.User
                .FindFirst("Id").Value);

            return _context.Carts
                           .Where(c => c.UserId == userId)
                           .ToList();
        }

        public Cart UpdateCart(int cartId, int quantity)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart == null) return null;

            cart.Quantity = quantity;
            _context.SaveChanges();
            return cart;
        }

        public bool DeleteCartItem(int cartId)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart == null) return false;

            // ensure ownership
            var userId = int.Parse(_httpContextAccessor.HttpContext.User
                                 .FindFirst("Id").Value);
            if (cart.UserId != userId)
                throw new Exception("Unauthorized");

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return true;
        }
    }
}
