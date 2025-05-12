using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepository
    {
        Cart AddToCart(CartRequestModel model);
        IEnumerable<Cart> GetCartItems();
        Cart UpdateCart(int cartId, int quantity);
        bool DeleteCartItem(int cartId);
    }
}
