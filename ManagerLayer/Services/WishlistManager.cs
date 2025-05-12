using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class WishlistManager : IWishlistManager
    {
        private readonly IWishlistRepository _wishlistRepository;
        public WishlistManager(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }
        public IEnumerable<WishlistItemModel> GetAllWishlists()
        {
            return _wishlistRepository.GetAllWishlists();
        }
        public WishlistEntity AddToWishlist(WishlistRequestModel wishlistRequestModel)
        {
            return _wishlistRepository.AddToWishlist(wishlistRequestModel);
        }
        public bool RemoveFromWishlist(int wishlistId)
        {
            return _wishlistRepository.RemoveFromWishlist(wishlistId);
        }
    }
}
