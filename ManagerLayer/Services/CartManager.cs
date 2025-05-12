using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class CartManager: ICartManager
    {
        private readonly ICartRepository _repo;
        public CartManager(ICartRepository repo) => _repo = repo;

        public Cart AddToCart(CartRequestModel m)
            => _repo.AddToCart(m);

        public IEnumerable<Cart> GetCartItems()
            => _repo.GetCartItems();

        public Cart UpdateCart(int bookid, int qty)
            => _repo.UpdateCart(bookid, qty);

        public bool DeleteCartItem(int id)
            => _repo.DeleteCartItem(id);
    } 
}
