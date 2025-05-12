using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System.Linq;

namespace BookStore.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _mng;
        public OrderController(IOrderManager mng) => _mng = mng;

        [HttpPost("purchase")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Purchase()
        {
            try
            {
                var order = _mng.CreateOrderFromCart();
                return Ok(new ResponseModel<Order>
                {
                    IsSuccess = true,
                    Message = "Order placed successfully",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetMyOrders()
        {
            var orders = _mng.GetAllOrders();
            return Ok(new ResponseModel<IEnumerable<Order>>
            {
                IsSuccess = true,
                Message = orders.Any()
                              ? "Orders retrieved"
                              : "No orders found",
                Data = orders
            });
        }
    }
}
