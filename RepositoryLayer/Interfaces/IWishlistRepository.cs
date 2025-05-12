using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IWishlistRepository
    {
        IEnumerable<WishlistItemModel> GetAllWishlists();
        WishlistEntity AddToWishlist(WishlistRequestModel wishlistRequestModel);
        bool RemoveFromWishlist(int wishlistId);
    }
}
