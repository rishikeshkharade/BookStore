using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRepository
    {
        Order CreateOrderFromCart();
        IEnumerable<Order> GetAllOrders();
    }
}
