using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IOrderManager
    {
        Order CreateOrderFromCart();
        IEnumerable<Order> GetAllOrders();
    }
}
