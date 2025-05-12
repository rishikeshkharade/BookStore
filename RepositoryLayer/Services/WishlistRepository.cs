using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistRepository(
            BookStoreDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<WishlistItemModel> GetAllWishlists()
        {
            var userId = int.Parse(
              _httpContextAccessor.HttpContext.User
                .FindFirst("Id").Value);

            return _context.Wishlists
                 .Where(w => w.UserId == userId)
                 .Include(w => w.Book)
                 .Select(w => new WishlistItemModel
                 {
                     WishlistId = w.WishlistId,
                     BookId = w.BookId,
                     BookName = w.Book.BookName,
                     Author = w.Book.Author,
                     Price = w.Book.Price,
                     DiscountPrice = w.Book.DiscountPrice
                 })
                 .ToList();
        }

        public WishlistEntity AddToWishlist(WishlistRequestModel model)
        {
            var userId = int.Parse(
              _httpContextAccessor.HttpContext.User
                .FindFirst("Id").Value);

            // 1) Prevent duplicates
            if (_context.Wishlists.Any(w => w.UserId == userId && w.BookId == model.BookId))
                throw new Exception("Book is already in wishlist");

            // 2) Create new
            var item = new WishlistEntity
            {
                UserId = userId,
                BookId = model.BookId
            };
            _context.Wishlists.Add(item);
            _context.SaveChanges();
            return item;
        }

        public bool RemoveFromWishlist(int wishlistId)
        {
            var item = _context.Wishlists.Find(wishlistId);
            if (item == null) return false;

            var userId = int.Parse(
              _httpContextAccessor.HttpContext.User
                .FindFirst("Id").Value);
            if (item.UserId != userId)
                throw new Exception("Unauthorized");

            _context.Wishlists.Remove(item);
            _context.SaveChanges();
            return true;
        }
    }
}
