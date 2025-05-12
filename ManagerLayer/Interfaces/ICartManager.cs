using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface ICartManager
    {
        Cart AddToCart(CartRequestModel model);
        Cart UpdateCart(int cartId, int quantity);
        bool DeleteCartItem(int cartId);
        IEnumerable<Cart> GetCartItems();
    }
}
