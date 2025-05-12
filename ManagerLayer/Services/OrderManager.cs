using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Order CreateOrderFromCart()
        {
            return _orderRepository.CreateOrderFromCart();
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }
    }
}
