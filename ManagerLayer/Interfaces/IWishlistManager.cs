using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IWishlistManager
    {
        IEnumerable<WishlistItemModel> GetAllWishlists();
        WishlistEntity AddToWishlist(WishlistRequestModel wishlistRequestModel);
        bool RemoveFromWishlist(int wishlistId);
    }
}
